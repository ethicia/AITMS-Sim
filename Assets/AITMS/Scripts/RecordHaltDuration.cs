using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kawaiiju.Traffic;
using System;

namespace Kawaiiju
{

    public enum Status { Moving, Halt };

    public class RecordHaltDuration : MonoBehaviour
    {

        public UnityEngine.AI.NavMeshAgent agent;
        public float movingSpeed = 1;
        public float minHaltDuration = 2.0f;
        [HideInInspector] public Status status;
        [HideInInspector] public float totalHaltDuration;
        [HideInInspector] public int haltCount;
        [HideInInspector] public float avgHaltDuration;
        private long stopTime;


        // Start is called before the first frame update
        void Start()
        {
            status = Status.Moving;
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            totalHaltDuration = 0;
            haltCount = 0;
            avgHaltDuration = 0;
        }

        // Update is called once per frame
        void Update()
        {
            float velocity = agent.velocity.magnitude;

            switch (status)
            {
                case Status.Moving:
                    {
                        //check for halt
                        if (velocity <= movingSpeed)
                        {
                            status = Status.Halt;
                            stopTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        }
                        break;
                    }

                case Status.Halt:
                    {
                        //check for movement
                        if (velocity > movingSpeed)
                        {
                            status = Status.Moving;
                            //calculate halt duration
                            float haltDuration = (DateTimeOffset.Now.ToUnixTimeMilliseconds() - stopTime) / 1000.0f;
                            if (haltDuration >= minHaltDuration)
                            {
                                totalHaltDuration += haltDuration;
                                haltCount++;
                                avgHaltDuration = totalHaltDuration / haltCount;

                                //Debug.Log("Halt Duration: " + haltDuration);
                                //Debug.Log("Total Duration: " + totalHaltDuration);
                                //Debug.Log("Halt Count: " + haltCount);
                                //Debug.Log("Avg Duration: " + avgHaltDuration);

                            }
                        }
                        break;
                    }
            }
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
