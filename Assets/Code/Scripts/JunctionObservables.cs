using System;
using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class JunctionObservables : MonoBehaviour
    {
        public int[] phaseTimings;
        public int[] phaseCounts;
        public int minimumPhaseInterval = 5;
        public int maximumPhaseInterval = 25;
        JunctionObservables()
        {
            phaseTimings = new int[4] { 13, 13, 13, 13 };
            phaseCounts = new int[4] { 0, 0, 0, 0 };
        }

        public void setPhaseInterval(int phase, int phaseInterval)
        {
            //Debug.Log("set phase:" + phase.ToString() + " interval:" + phaseInterval.ToString());
            //boundary conditions
            phaseInterval = Math.Max(phaseInterval, minimumPhaseInterval);
            phaseInterval = Math.Min(phaseInterval, maximumPhaseInterval);

            phaseTimings[phase] = phaseInterval;

        }

    }
}