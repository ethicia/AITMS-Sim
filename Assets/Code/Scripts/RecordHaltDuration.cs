using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Kawaiiju.Traffic
{

    public enum Status { Moving, Halt };

    public class RecordHaltDuration : MonoBehaviour
    {

        [HideInInspector] public UnityEngine.AI.NavMeshAgent agent;
        public float movingSpeed = 1;
        public float minHaltDuration = 2.0f;
        [HideInInspector] public Status status;
        [HideInInspector] public float totalHaltDuration;
        [HideInInspector] public int haltCount;
        [HideInInspector] public float avgHaltDuration;
        public float haltPunishment;
        public float haltThreshold = 60.0f;
        private long stopTime;
        private float haltDuration;


        // Start is called before the first frame update
        void Start()
        {
            status = Status.Moving;
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            totalHaltDuration = 0;
            haltCount = 0;
            avgHaltDuration = 0;
            haltDuration = 0;
            haltPunishment = 0;
        }

        // Update is called once per frame
        void Update()
        {
            float velocity = agent.velocity.magnitude;
            haltDuration += Time.deltaTime;
            if (haltDuration >= haltThreshold)
            {
                Debug.Log("halt threshold reached; deleting vehicle");
                Destroy(this.gameObject);
            }

            switch (status)
            {
                case Status.Moving:
                    {
                        haltDuration = 0;
                        //check for halt
                        if (velocity <= movingSpeed)
                        {
                            status = Status.Halt;
                        }
                        break;
                    }

                case Status.Halt:
                    {
                        //check for movement
                        if (velocity > movingSpeed)
                        {
                            status = Status.Moving;

                            //add halt duration to total halt duration
                            if (haltDuration >= minHaltDuration)
                            {
                                haltPunishment += haltDuration;
                                totalHaltDuration += haltDuration;
                                haltCount++;
                                avgHaltDuration = totalHaltDuration / haltCount;
                                haltDuration = 0;
                            }
                        }
                        break;
                    }
            }
        }

        public float getHaltPunishment()
        {
            float temp = -1 * haltPunishment;
            haltPunishment = 0;
            return temp;
        }

        public float getAvgHaltDuration()
        {
            return avgHaltDuration;
        }

        public void reset()
        {
            status = Status.Moving;
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            totalHaltDuration = 0;
            haltCount = 0;
            avgHaltDuration = 0;
        }
    }
}
