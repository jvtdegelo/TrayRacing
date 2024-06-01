using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;

public class CarAgent : Agent
{

    private Transform spawnPoint;
    private CarController carController;
    private float reward = 0f;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        spawnPoint = transform;
        Debug.Log("initial: " + spawnPoint.position);
    }

    public override void OnEpisodeBegin()
    {
        carController.ReturnToSpawn();
        carController.ResetCheckpoints();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        RaycastHit nextCheckpoint = carController.GetNextCheckpoint();
        sensor.AddObservation(nextCheckpoint.distance);

        float directionDot = Math.Abs(Vector3.Dot(transform.forward, nextCheckpoint.normal));
        sensor.AddObservation(directionDot);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            EndEpisode();
        if (Input.GetKeyDown(KeyCode.P))
            Debug.Log(transform.name + " has " + reward + " points");
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // // TODO: verificar se é melhor discrete ou continuous
        // float verticalInput = 0f;
        // float horizontalInput = 0f;

        // switch (actions.DiscreteActions[0])
        // {
        //     case 0: verticalInput = 0f; break;
        //     case 1: verticalInput = +1f; break;
        //     case 2: verticalInput = -1f; break;
        // }

        // switch (actions.DiscreteActions[1])
        // {
        //     case 0: horizontalInput = 0f; break;
        //     case 1: horizontalInput = +1f; break;
        //     case 2: horizontalInput = -1f; break;
        // }

        // carController.SetInputs(verticalInput, horizontalInput);

        // OR
        // TODO: acho que esse faz mais sentido porque o do player é contínuo, e transita entre -1 e 1 a depender de quanto tempo está apertando

        float verticalInput = actions.ContinuousActions[0];
        float horizontalInput = actions.ContinuousActions[1];
        carController.SetInputs(verticalInput, horizontalInput);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // base.Heuristic(actionsOut);
        // int verticalAction = 0;
        // if (Input.GetKeyDown(KeyCode.UpArrow)) verticalAction = 1;
        // if (Input.GetKeyDown(KeyCode.DownArrow)) verticalAction = 2;

        // int horizontalAction = 0;
        // if (Input.GetKeyDown(KeyCode.RightArrow)) horizontalAction = 1;
        // if (Input.GetKeyDown(KeyCode.LeftArrow)) horizontalAction = 2;

        // ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        // discreteActions[0] = verticalAction;
        // discreteActions[1] = horizontalAction;

        // OR

        float verticalAction = Input.GetAxis("Vertical");
        float horizontalAction = Input.GetAxis("Horizontal");

        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = verticalAction;
        continousActions[1] = horizontalAction;
    }

    public void AddRewardDebug(float reward) { this.reward += reward; }
}
