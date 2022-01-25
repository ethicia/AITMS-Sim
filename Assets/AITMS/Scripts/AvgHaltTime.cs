using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kawaiiju.Traffic;
using System.IO;

namespace Kawaiiju
{
    public class AvgHaltTime : MonoBehaviour
    {
        public GameObject pool;

        public int carCount;
        public float globalHaltAverage;
        public string dataFilePath;
        public float dataInterval = 15;

        private float timer;

        // Start is called before the first frame update
        void Start()
        {
            //pool = GameObject.Find("/AITMS-Demo/TrafficSystem/Pool");
            timer = dataInterval;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                //append data to csv
                string entry = string.Format("{0},{1},{2}\n", Time.fixedTime, carCount, globalHaltAverage);
                File.AppendAllText(dataFilePath, entry);

                //reset timer
                timer = dataInterval;
            }
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
