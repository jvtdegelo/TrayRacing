using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCheckpointTrigger : MonoBehaviour
{
    public GameObject thisCheckpoint, nextCheckpoint;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checkpoint " + this.name + " triggered");
        if (other.TryGetComponent(out CarPointer carPointer))
            if (carPointer.GetCar().TryGetComponent(out CarController carController))
                carController.SetLastCheckpointPosition(transform.position, transform.rotation);

        thisCheckpoint.SetActive(false);
        nextCheckpoint.SetActive(true);
    }
}
