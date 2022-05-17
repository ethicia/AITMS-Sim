using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class GlobalObservables : MonoBehaviour
    {
        // Start is called before the first frame update
        public GameObject[] junctions;
        public int[] observables;

        void Start()
        {
            observables = new int[junctions.Length * 9];
        }

        // Update is called once per frame
        void Update()
        {

        }

        public int[] getObservables()
        {
            int i = 0;
            foreach (GameObject junction in junctions)
            {
                JunctionObservables temp = junction.GetComponent<JunctionObservables>();
                observables[i++] = temp.currentPhase;
                for (int j = 0; j < 4; j++)
                    observables[i++] = temp.phaseTimings[j];
                for (int j = 0; j < 4; j++)
                    observables[i++] = temp.phaseCounts[j];

            }
            return observables;
        }

    }
}