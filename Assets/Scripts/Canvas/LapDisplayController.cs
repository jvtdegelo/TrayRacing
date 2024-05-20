using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapDisplayController : MonoBehaviour
{

    public GameObject minuteDisplay, secondDisplay, milliDisplay;
    public LapTimerController lapTimer;
    private TMPro.TextMeshProUGUI secondDisplayText, minuteDisplayText, milliDisplayText;

    void Start()
    {
        secondDisplayText = secondDisplay.GetComponent<TMPro.TextMeshProUGUI>();
        minuteDisplayText = minuteDisplay.GetComponent<TMPro.TextMeshProUGUI>();
        milliDisplayText = milliDisplay.GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void UpdateTimer()
    {
        // Debug.Log(this.name + " updated");
        // Debug.Log(LapTimerController.minuteCount + "." + LapTimerController.secondCount + "." + LapTimerController.milliCount);
        secondDisplayText.text =
            (LapTimerController.secondCount <= 9 ? "0" : "")
            + LapTimerController.secondCount + ".";

        minuteDisplayText.text =
            (LapTimerController.minuteCount <= 9 ? "0" : "")
            + LapTimerController.minuteCount + ".";

        milliDisplayText.text = "" + LapTimerController.milliCount;
    }

    void Update() { }
}
