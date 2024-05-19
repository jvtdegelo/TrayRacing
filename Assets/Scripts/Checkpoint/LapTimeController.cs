using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimeController : MonoBehaviour {
    public static int minuteCount;
    public static int secondCount;
    public static int milliCount;
    public GameObject minuteBox;
    public GameObject secondBox;
    public GameObject milliBox;
    private float elapsedTime = 0f;

    void Update () {
        elapsedTime += Time.deltaTime;
        milliCount = Mathf.FloorToInt(((elapsedTime * 1000F) % 1000F) / 100);

        milliBox.GetComponent<TMPro.TextMeshProUGUI>().text = "" + milliCount;
        
        secondCount = Mathf.FloorToInt(elapsedTime % 60F);
        if (secondCount <= 9) {
            secondBox.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + secondCount + ".";
        } else {
            secondBox.GetComponent<TMPro.TextMeshProUGUI>().text = "" + secondCount + ".";
        }

        minuteCount = Mathf.FloorToInt(elapsedTime / 60F);
        if (minuteCount <= 9) {
            minuteBox.GetComponent<TMPro.TextMeshProUGUI>().text = "0" + minuteCount + ":";
        } else {
            minuteBox.GetComponent<TMPro.TextMeshProUGUI>().text = "" + minuteCount + ":";
        }
    }

    public void resetTimer()
    {
        elapsedTime = 0;
    }

}