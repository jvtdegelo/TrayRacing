using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapTimerController : MonoBehaviour
{
    public static int minuteCount, secondCount, milliCount;
    public LapDisplayController mainLapDisplay;
    private float elapsedTimeSeconds = 0f;

    void Start() { }

    void Update()
    {
        elapsedTimeSeconds += Time.deltaTime;

        minuteCount = Mathf.FloorToInt(elapsedTimeSeconds / 60F);
        secondCount = Mathf.FloorToInt(elapsedTimeSeconds % 60F);
        milliCount = Mathf.FloorToInt(((elapsedTimeSeconds * 1000F) % 1000F) / 100);

        mainLapDisplay.UpdateTimer();
    }

    public void ResetTimer()
    {
        elapsedTimeSeconds = 0;
    }

}