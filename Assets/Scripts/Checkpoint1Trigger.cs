using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint1Trigger : MonoBehaviour {
    public GameObject checkpoint1Trig;
    public GameObject checkpoint2Trig;
    public GameObject checkpoint3Trig;
    public GameObject checkpoint4Trig;

    public GameObject minuteDisplay;
    public GameObject secondDisplay;
    public GameObject milliDisplay;

    public LapTimeController lapTimeController;

    private bool isFirstTimeTriggered = true;

    void OnTriggerEnter () {
        if (isFirstTimeTriggered == false) {
            insertLapTime();
        }
        isFirstTimeTriggered = false;
        checkpoint1Trig.SetActive (false);
        checkpoint2Trig.SetActive (true);
        checkpoint3Trig.SetActive (false);
        checkpoint4Trig.SetActive (false);

    }

    private void insertLapTime() {
        if (LapTimeController.secondCount <= 9) {
            secondDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + LapTimeController.secondCount + ".";
        } else {
            secondDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeController.secondCount + ".";
        }

        if (LapTimeController.minuteCount <= 9) {
            minuteDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + LapTimeController.minuteCount + ":";
        } else {
            minuteDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeController.minuteCount + ":";
        }

        milliDisplay.GetComponent<TMPro.TextMeshProUGUI>().text = "" + LapTimeController.milliCount;

        lapTimeController.resetTimer();

    }
}
