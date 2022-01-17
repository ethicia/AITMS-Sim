using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kawaiiju.Traffic;

namespace Kawaiiju
{
    public class AvgHaltTime : MonoBehaviour
    {
        public GameObject pool;

        public int carCount;
        public float globalHaltAverage;

        // Start is called before the first frame update
        void Start()
        {
            //pool = GameObject.Find("/AITMS-Demo/TrafficSystem/Pool");
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            float totalAvg = 0;
            carCount = pool.transform.childCount;
            for (int i = 0; i < carCount; i++)
            {
                float childAvg = pool.transform.GetChild(i).GetComponent<RecordHaltDuration>().getAvgHaltDuration();
                //Debug.Log(childAvg);
                totalAvg += childAvg;
            }

            if (carCount > 0)
                globalHaltAverage = totalAvg / carCount;

            //Debug.Log("Car Count: " + carCount);
            //Debug.Log("Avg: " + globalHaltAverage);
        }
    }
}
