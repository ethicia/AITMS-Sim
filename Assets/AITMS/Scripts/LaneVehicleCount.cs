using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Kawaiiju
{
    public class LaneVehicleCount : MonoBehaviour
    {

        public int vehiclewithin = 0;
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Gib")
            {
                vehiclewithin++;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Gib")
            {
                vehiclewithin--;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}