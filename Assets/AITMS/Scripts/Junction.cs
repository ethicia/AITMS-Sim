using System;
using System.Collections;
using System.Collections.Generic;
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
        public float minimumPhaseInterval = 3;
        public float maximumPhaseInterval = 10;
        public float averageTime = 1;
        public int firstLaneCount = 0;
        public int secondLaneCount = 0;
        public int thirdLaneCount = 0;
        public int fourthLaneCount = 0;
        public bool debug = false;
        [HideInInspector] public GameObject another;
        public LaneVehicleCount firstLaneBox;
        public LaneVehicleCount secondLaneBox;
        public LaneVehicleCount thirdLaneBox;
        public LaneVehicleCount fourthLaneBox;
        // -------------------------------------------------------------------
        // Initialization

        public override void Start()
        {
            phaseInterval = 5;
            base.Start();
            if (phases.Length > 0)
                phases[0].Enable();
            foreach (JunctionTrigger jt in triggers)
                jt.junction = this;
            //another = GameObject.FindGameObjectWithTag("FirstLane");
            //firstLaneBox = another.GetComponent<LaneVehicleCount>();
            //another = GameObject.FindGameObjectWithTag("SecondLane");
            //secondLaneBox = another.GetComponent<LaneVehicleCount>();
            //another = GameObject.FindGameObjectWithTag("ThirdLane");
            //thirdLaneBox = another.GetComponent<LaneVehicleCount>();
            //another = GameObject.FindGameObjectWithTag("FourthLane");
            //fourthLaneBox = another.GetComponent<LaneVehicleCount>();

        }

        // -------------------------------------------------------------------
        // Update

        private void Update()
        {
            if (type == PhaseType.Timed)
            {
                m_PhaseTimer += Time.deltaTime;
                if (!m_PhaseEnded && m_PhaseTimer > (phaseInterval * 0.5f))
                    EndPhase();
                if (m_PhaseTimer > phaseInterval)
                    ChangePhase();
            }
            if(firstLaneBox != null)
            firstLaneCount = firstLaneBox.vehiclewithin;
            if(secondLaneBox != null)
            secondLaneCount = secondLaneBox.vehiclewithin;
            if(thirdLaneBox != null)
            thirdLaneCount = thirdLaneBox.vehiclewithin;
            if(fourthLaneBox != null)
            fourthLaneCount = fourthLaneBox.vehiclewithin;
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
            
            //calculating phase interval
            switch(m_CurrentPhase){
                case 0: phaseInterval = (firstLaneCount * averageTime)/5;
                if(debug)
                Debug.Log("count " + firstLaneCount);
                break;
                
                case 1: phaseInterval = (secondLaneCount * averageTime)/5;
                if(debug)
                Debug.Log("count " + secondLaneCount);
                break;

                case 3: phaseInterval = (thirdLaneCount * averageTime)/5;
                if(debug)
                Debug.Log("count " + thirdLaneCount);
                break;

                case 2: phaseInterval = (fourthLaneCount * averageTime)/5;
                if(debug)
                Debug.Log("count " + fourthLaneCount);
                break;
            }

            //boundary condition
            phaseInterval = Math.Max(phaseInterval,minimumPhaseInterval);
            phaseInterval = Math.Min(phaseInterval,maximumPhaseInterval);

            phases[m_CurrentPhase].Enable();

            if(debug){
                Debug.Log("changing to phase " + m_CurrentPhase);
                Debug.Log("phase interval "+ phaseInterval.ToString());
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
