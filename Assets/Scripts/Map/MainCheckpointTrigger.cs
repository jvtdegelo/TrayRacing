using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainCheckpointTrigger : MonoBehaviour
{
    public GameObject thisCheckpoint, nextCheckpoint;
    public GameObject[] otherCheckpoints;

    // Se for ter mais de um lapDisplay (para varios laps), só botar como array, e fazer o updateTimer pelo indice == lap
    public LapDisplayController lapDisplay;
    public LapTimerController lapTimerController;

    private bool isFirstTimePlayerTriggered = true;

    public GameObject finishPanel;
    public GameObject fpsCounter;
    public GameObject lapDisplayUI;

    public bool hasPlayerWon = true;
    public float playerFinishTime;
    public int indexTime = 0;
    private TMPro.TextMeshProUGUI finishTimesText, winOrLoseText;

    int botFinishIndex = 1;

    public GameObject playerCar;

    private PlayerInput playerInput;
    public GameObject audioParent;

    private AudioSource[] allAudioSources;

    void Start() {
        finishPanel.SetActive(false);
        finishTimesText = finishPanel.transform.Find("FinishTimesText").GetComponent<TMPro.TextMeshProUGUI>();
        winOrLoseText = finishPanel.transform.Find("WinLoseText").GetComponent<TMPro.TextMeshProUGUI>();
        
        if (audioParent != null)
        {
            allAudioSources = audioParent.GetComponentsInChildren<AudioSource>();
        }

        SetInitialFinishTimeText();
    }

    void OnTriggerEnter(Collider car)
    {
        Debug.Log(this.name + " triggered, firstTime: " + isFirstTimePlayerTriggered + " | tag: " + car.tag);
        if (car.CompareTag("AI") && isFirstTimePlayerTriggered == false)
        {
            if (botFinishIndex < 5)
            {
                Debug.Log("opa bao " + botFinishIndex);
                finishTimesText.text += "AI" + botFinishIndex + ": ";
                finishTimesText.text += Time.timeSinceLevelLoad.ToString("F2") + "s\n";
                botFinishIndex++;
                hasPlayerWon = false;
            }
        }

        if (car.CompareTag("Player"))
        {
            if (isFirstTimePlayerTriggered == false)
            {
                playerFinishTime = Time.timeSinceLevelLoad;
                finishPanel.SetActive(true);
                ShowFinishPanel();
                // lapDisplay.UpdateTimer();
                // lapTimerController.ResetTimer();
            }

            isFirstTimePlayerTriggered = false;
            thisCheckpoint.SetActive(false);
            nextCheckpoint.SetActive(true);

            foreach (GameObject checkpoint in otherCheckpoints)
            {
                checkpoint.SetActive(false);
            }
        }

    }

    private void SetInitialFinishTimeText()
    {
        finishTimesText.text = "Finish times:\n";
    }


    private void ShowFinishPanel()
    {           
        playerInput = playerCar.GetComponent<PlayerInput>();
        playerInput.enabled = false;
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }


        fpsCounter.SetActive(false);
        lapDisplayUI.SetActive(false);

        finishPanel.SetActive(true);
    
        finishTimesText.text += "You: ";
        finishTimesText.text += playerFinishTime.ToString("F2") + "s\n";

        winOrLoseText.text = hasPlayerWon ? "You Won!" : "You Lost!";

    }

}
