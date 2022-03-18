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
        private float phaseInterval = 5;
        public float yellowTime = 5;
        public JunctionObservables junctionObservables;
        public bool debug = false;
        [HideInInspector] public GameObject another;
        public LaneVehicleCount[] laneBox;
        public TextMeshPro[] textOnCube;
        public float timeRemaining;
        public bool timerIsRunning = false;
        // -------------------------------------------------------------------
        // Initialization

        public override void Start()
        {
            timeRemaining = phaseInterval;

            base.Start();
            if (phases.Length > 0)
                phases[0].Enable();
            foreach (JunctionTrigger jt in triggers)
                jt.junction = this;
        }

        // -------------------------------------------------------------------
        // Update

        private void Update()
        {
            if (type == PhaseType.Timed)
            {
                m_PhaseTimer += Time.deltaTime;

                //fixed yellow time
                if (!m_PhaseEnded && m_PhaseTimer > (phaseInterval - yellowTime))
                    EndPhase();
                if (m_PhaseTimer > phaseInterval)
                    ChangePhase();
            }

            //updating vehicle counts
            for (int i = 0; i < laneBox.Length; i++)
            {
                junctionObservables.phaseCounts[i] = laneBox[i].vehiclewithin;
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
            if (m_CurrentPhase < phases.Length - 1)
                m_CurrentPhase++;
            else
                m_CurrentPhase = 0;

            //assigning phase intervals
            for (int i = 0; i < laneBox.Length; i++)
            {
                phaseInterval = junctionObservables.phaseTimings[i];
            }

            timeRemaining = phaseInterval;

            phases[m_CurrentPhase].Enable();

            if (debug)
            {
                Debug.Log("changing to phase " + m_CurrentPhase);
                Debug.Log("phase interval " + phaseInterval.ToString());
                //Debug.Log("vehicle count "+)
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
