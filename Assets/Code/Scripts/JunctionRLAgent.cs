using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class JunctionRLAgent : Agent
{
    private Kawaiiju.Traffic.JunctionObservables junctionObservables;

    // Start is called before the first frame update
    void Start()
    {
        junctionObservables = gameObject.GetComponent<Kawaiiju.Traffic.JunctionObservables>();
    }

    public override void OnEpisodeBegin()
    {
        Debug.Log("New Episode");
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //current phase intervals
        sensor.AddObservation(junctionObservables.phaseTimings[0]);
        sensor.AddObservation(junctionObservables.phaseTimings[1]);
        sensor.AddObservation(junctionObservables.phaseTimings[2]);
        sensor.AddObservation(junctionObservables.phaseTimings[3]);

        //phase count
        sensor.AddObservation(junctionObservables.phaseCounts[0]);
        sensor.AddObservation(junctionObservables.phaseCounts[1]);
        sensor.AddObservation(junctionObservables.phaseCounts[2]);
        sensor.AddObservation(junctionObservables.phaseCounts[3]);

    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        for (int i = 0; i < 4; i++)
        {
            float scaledInterval = ScaleAction(Mathf.Clamp(actionBuffers.ContinuousActions[i], -1f, 1f), 5f, 35f);
            junctionObservables.setPhaseInterval(i, Mathf.RoundToInt(scaledInterval));
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

}
