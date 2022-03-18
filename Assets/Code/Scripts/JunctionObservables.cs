using UnityEngine;

namespace Kawaiiju.Traffic
{
    public class JunctionObservables : MonoBehaviour
    {
        public int[] phaseTimings;
        public int[] phaseCounts;
        public float minimumPhaseInterval = 5;
        public float maximumPhaseInterval = 25;
        JunctionObservables()
        {
            phaseTimings = new int[4] { 15, 15, 15, 15 };
            phaseCounts = new int[4] { 0, 0, 0, 0 };
        }

    }
}