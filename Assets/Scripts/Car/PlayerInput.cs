using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class PlayerInput : MonoBehaviour
{
    private CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        carController.SetInputs(verticalInput, horizontalInput);

        // returns to last checkpoint when R is pressed
        if (Input.GetKeyDown(KeyCode.R))
            carController.ReturnToLastCheckpoint();
    }
}
