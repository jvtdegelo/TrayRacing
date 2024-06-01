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

    private RayPerceptionSensorComponent3D rayPerception;

    private void Awake()
    {
        carController = GetComponent<CarController>();
        spawnPoint = transform;
    }

    public override void OnEpisodeBegin()
    {
        transform.position = spawnPoint.position;
        transform.forward = spawnPoint.forward;
        carController.StopCompletely();
        // base.OnEpisodeBegin();
    }

    // TODO: aqui ele adicionava o Vector3.Dot entre o transform.forward e o nextCheckpoint, mas não temos nextCheckpoint
    public override void CollectObservations(VectorSensor sensor)
    {
        RaycastHit nextCheckpoint = carController.GetNextCheckpoint();
        sensor.AddObservation(nextCheckpoint.distance);

        float directionDot = Math.Abs(Vector3.Dot(transform.forward, nextCheckpoint.normal));
        sensor.AddObservation(directionDot);
        // Debug.Log(nextCheckpoint.distance);
        // Debug.Log(nextCheckpoint.normal);
        // Debug.Log(nextCheckpoint);
        // Debug.Log("");
        // SE EU COLOCAR UM RAY POINT QUE SAI DO CARRO E VAI PARA FORWARD, E ELE BATER EM UM CHECKPOINT, E EU ALIMENTAR ISSO PARA A IA, TALVEZ FUNCIONE
        // sensor.AddObservation(transform.position);
        // sensor.AddObservation(targetTransform.position);

        // base.CollectObservations(sensor);
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
}
