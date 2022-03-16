using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class TrafficLight : MonoBehaviour
    {

        public Light RedLight;
        public Light GreenLight;
        public void SetLight(bool input)
        {
            if (input)
            {
                //green on
                GreenLight.intensity = 1.5f;
                RedLight.intensity = 0;
            }
            else
            {
                GreenLight.intensity = 0;
                RedLight.intensity = 1;
            }

        }
    }
}
