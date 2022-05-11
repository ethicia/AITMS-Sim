using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class Collector : MonoBehaviour
    {
        private JunctionRLAgent RLAgent;
        private float totalHaltTime = 0f;
        private int vehicleCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            RLAgent = gameObject.GetComponentInParent<JunctionRLAgent>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Gib" && RLAgent != null)
            {
                float haltDuration = other.gameObject.GetComponent<RecordHaltDuration>().getHaltPunishment();
                totalHaltTime += haltDuration;
                vehicleCount++;
            }
        }

        public float getAvgHaltTime()
        {

            if (vehicleCount == 0) return 0f;

            float temp = totalHaltTime / vehicleCount;

            totalHaltTime = 0f;
            vehicleCount = 0;

            return temp;
        }
    }
}
