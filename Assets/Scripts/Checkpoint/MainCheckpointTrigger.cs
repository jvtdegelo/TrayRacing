using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCheckpointTrigger : MonoBehaviour
{
    public GameObject thisCheckpoint, nextCheckpoint;
    public GameObject[] otherCheckpoints;

    // Se for ter mais de um lapDisplay (para varios laps), só botar como array, e fazer o updateTimer pelo indice == lap
    public LapDisplayController lapDisplay;
    public LapTimerController lapTimerController;

    private bool isFirstTimeTriggered = true;

    void Start() { }

    void OnTriggerEnter()
    {
        Debug.Log(this.name + " triggered, firstTime: " + isFirstTimeTriggered);
        // inserts lap time when it is not first time triggered
        if (isFirstTimeTriggered == false)
        {
            lapDisplay.UpdateTimer();
            lapTimerController.ResetTimer();
        }

        isFirstTimeTriggered = false;
        thisCheckpoint.SetActive(false);
        nextCheckpoint.SetActive(true);

        foreach (GameObject checkpoint in otherCheckpoints)
        {
            checkpoint.SetActive(false);
        }
    }
}
