using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCheckpointTrigger : MonoBehaviour
{
    public GameObject thisCheckpoint, nextCheckpoint;
    void OnTriggerEnter()
    {
        Debug.Log("Checkpoint " + this.name + " triggered");
        thisCheckpoint.SetActive(false);
        nextCheckpoint.SetActive(true);
    }
}
