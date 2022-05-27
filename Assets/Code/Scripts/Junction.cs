using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class Junction : Road
    {
        // -------------------------------------------------------------------
        // Enum

        public enum PhaseType { Timed, OnDemand }

        // -------------------------------------------------------------------
        // Properties

        [Header("Junction")]
        public PhaseType type = PhaseType.Timed;
        public Phase[] phases;
        public JunctionTrigger[] triggers;
        private float phaseInterval = 15;
        public float yellowTime = 5;
        [HideInInspector] public JunctionObservables junctionObservables;
        public bool debug = false;
        [HideInInspector] public GameObject another;
        public LaneVehicleCount[] laneBox;
        public TextMeshPro[] textOnCube;
        public float timeRemaining;
        public bool timerIsRunning = false;
        private JunctionRLAgent RLAgent;
        private Collector collector;
        private float minCycle;
        private float maxCycle;
        // -------------------------------------------------------------------
        // Initialization

        public override void Start()
        {
            timeRemaining = phaseInterval;
            junctionObservables = gameObject.GetComponent<JunctionObservables>();
            RLAgent = gameObject.GetComponent<JunctionRLAgent>();
            collector = gameObject.GetComponentInChildren<Collector>();

            minCycle = junctionObservables.minimumPhaseInterval * phases.Length;
            maxCycle = junctionObservables.maximumPhaseInterval * phases.Length;

            base.Start();
            if (phases.Length > 0)
                phases[0].Enable();
            foreach (JunctionTrigger jt in triggers)
                jt.junction = this;
        }

        // -------------------------------------------------------------------
        // Update

        private bool decisionTaken = false;
        float reward;
        int totalLaneCount;
        float scaledHaltTime;

        private void Update()
        {
            //updating vehicle counts
            for (int i = 0; i < laneBox.Length; i++)
            {
                junctionObservables.phaseCounts[i] = laneBox[i].vehiclewithin;
            }

            if (type == PhaseType.Timed)
            {
                m_PhaseTimer += Time.deltaTime;

                //fixed yellow time
                if (!m_PhaseEnded && m_PhaseTimer > (phaseInterval - yellowTime))
                {
                    if (RLAgent != null)
                    {
                        //scaling reward
                        if (minCycle != maxCycle)
                        {
                            scaledHaltTime = (collector.getAvgHaltTime(true) - minCycle) / (maxCycle - minCycle);

                            //using total vehicle count
                            //totalLaneCount = 0;
                            //for (int i = 0; i < phases.Length; i++)
                            //    totalLaneCount += laneBox[i].vehiclewithin;
                            //reward = scaledHaltTime - (totalLaneCount / phases.Length);

                            //using default reward mechanism
                            reward = scaledHaltTime - (laneBox[m_CurrentPhase].vehiclewithin / 8);
                        }
                        else
                            reward = 0f;

                        RLAgent.AddReward(reward);

                        //resetting after 4 phases
                        //if (m_CurrentPhase == 3)
                        //{
                        //    Debug.Log("cumulative reward: " + RLAgent.GetCumulativeReward());
                        //    RLAgent.EndEpisode();
                        //}

                        //resetting after every phase
                        if (debug)
                            Debug.Log("cumulative reward: " + RLAgent.GetCumulativeReward());
                        RLAgent.EndEpisode();
                    }
                    EndPhase();
                }
                if (m_PhaseTimer > phaseInterval)
                {
                    ChangePhase();
                    if (RLAgent != null)
                    {
                        RLAgent.RequestDecision();
                        Debug.Log("phase: " + m_CurrentPhase + " count: " + laneBox[m_CurrentPhase].vehiclewithin + " time: " + phaseInterval);
                    }
                }
            }

            //timer display
            if (timerIsRunning)
            {
                if (timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                }
                else
                {
                    if (debug)
                        Debug.Log("Timer Reset Indicator");
                    timeRemaining = 0;
                }
            }
        }

        // -------------------------------------------------------------------
        // Display Time

        void DisplayTime(float timeToDisplay)
        {
            timeToDisplay += 1;
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            foreach (TextMeshPro x in textOnCube)
                x.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // -------------------------------------------------------------------
        // Phase

        float m_PhaseTimer;
        bool m_PhaseEnded;
        private int m_CurrentPhase;

        private void EndPhase()
        {
            m_PhaseEnded = true;
            phases[m_CurrentPhase].End();
        }

        public void ChangePhase()
        {
            m_PhaseTimer = 0;
            m_PhaseEnded = false;
            m_CurrentPhase = (m_CurrentPhase + 1) % phases.Length;

            //assigning current phase
            junctionObservables.currentPhase = m_CurrentPhase;

            //assigning phase intervals
            for (int i = 0; i < phases.Length; i++)
            {
                phaseInterval = junctionObservables.phaseTimings[i];
            }

            timeRemaining = phaseInterval;

            phases[m_CurrentPhase].Enable();

            if (debug)
            {
                Debug.Log("changing to phase " + m_CurrentPhase);
                Debug.Log("phase interval " + phaseInterval.ToString());
                Debug.Log("vehicle count " + laneBox[m_CurrentPhase] + " " + laneBox[m_CurrentPhase].vehiclewithin);
            }
        }

        public void TryChangePhase()
        {
            if (!HasActiveTrains())
                ChangePhase();
        }

        // -------------------------------------------------------------------
        // Debug

        private Mesh m_Cube;
        public Mesh cube
        {
            get
            {
                if (m_Cube == null)
                {
                    GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    m_Cube = gameObject.GetComponent<MeshFilter>().sharedMesh;
                    GameObject.DestroyImmediate(gameObject);
                }
                return m_Cube;
            }
        }

        private void OnDrawGizmos()
        {
            if (TrafficSystem.Instance.drawGizmos)
            {
                Phase phase = phases[m_CurrentPhase];
                foreach (WaitZone zone in phase.positiveZones)
                {
                    Gizmos.color = zone.canPass ? Color.green : Color.red;
                    DrawAreaGizmo(zone.transform);
                }
                Gizmos.color = Color.red;
                foreach (WaitZone zone in phase.negativeZones)
                    DrawAreaGizmo(zone.transform);
            }
        }

        private void DrawAreaGizmo(Transform t)
        {
            Matrix4x4 rotationMatrix1 = Matrix4x4.TRS(t.position - new Vector3(0, t.localScale.y * 0.5f, 0), t.rotation, Vector3.Scale(t.lossyScale, new Vector3(1f, 0.1f, 1f)));
            Gizmos.matrix = rotationMatrix1;
            Gizmos.DrawWireMesh(cube, Vector3.zero, Quaternion.identity);
        }

        // -------------------------------------------------------------------
        // Data Classes

        [Serializable]
        public class Phase
        {
            public WaitZone[] positiveZones;
            public WaitZone[] negativeZones;
            public TrafficLight[] positiveLights;
            public TrafficLight[] negativeLights;

            public void Enable()
            {
                foreach (WaitZone zone in positiveZones)
                    zone.canPass = true;
                foreach (TrafficLight light in positiveLights)
                    light.SetLight(true);
                foreach (WaitZone zone in negativeZones)
                    zone.canPass = false;
                foreach (TrafficLight light in negativeLights)
                    light.SetLight(false);
            }

            public void End()
            {
                foreach (WaitZone zone in positiveZones)
                    zone.canPass = false;
            }
        }
    }
}
