using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

[RequireComponent(typeof(CarController))]
public class RandomInput : MonoBehaviour
{
    private CarController carController;

    void Start()
    {
        carController = GetComponent<CarController>();
    }

    void Update()
    {
        float verticalInput = Random.Range(-1f, 1f);
        float horizontalInput = Random.Range(-1f, 1f);

        carController.SetInputs(verticalInput, horizontalInput);

        // returns to last checkpoint when R is pressed
        if (Input.GetKeyDown(KeyCode.R))
            carController.ReturnToLastCheckpoint();
    }
}
