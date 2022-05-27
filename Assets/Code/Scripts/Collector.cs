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

        public float getAvgHaltTime(bool reset)
        {

            if (vehicleCount == 0) return 0f;

            float temp = totalHaltTime / vehicleCount;

            if (reset)
            {
                vehicleCount = 0;
                totalHaltTime = 0;
            }

            return temp;
        }

        public int getVehicleCount(bool reset)
        {
            int temp = vehicleCount;
            if (reset)
            {
                vehicleCount = 0;
                totalHaltTime = 0;
            }
            return temp;
        }
    }
}
