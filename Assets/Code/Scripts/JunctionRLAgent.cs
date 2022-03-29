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
        junctionObservables.setPhaseInterval(0, Mathf.RoundToInt(actionBuffers.ContinuousActions[0]));
        junctionObservables.setPhaseInterval(1, Mathf.RoundToInt(actionBuffers.ContinuousActions[1]));
        junctionObservables.setPhaseInterval(2, Mathf.RoundToInt(actionBuffers.ContinuousActions[2]));
        junctionObservables.setPhaseInterval(3, Mathf.RoundToInt(actionBuffers.ContinuousActions[3]));
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }

}
