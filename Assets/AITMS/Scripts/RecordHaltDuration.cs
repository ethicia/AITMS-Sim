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
        public float movingSpeed;
        public float minHaltDuration = 2.0f;
        public Status status;

        private long stopTime;
        private List<float> haltDurations;

        // Start is called before the first frame update
        void Start()
        {
            status = Status.Moving;
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            haltDurations = new List<float>();
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
                                Debug.Log(haltDuration);
                                haltDurations.Add(haltDuration);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
