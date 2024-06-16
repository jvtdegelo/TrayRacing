using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour
{

    public GameObject countDown;
    private TMPro.TextMeshProUGUI countdownText;
    public AudioSource getReady;
    public AudioSource goAudio;
    public GameObject lapTimer;
    public GameObject[] cars;
    private CarController[] carControllers;

    void Start()
    {
        carControllers = new CarController[cars.Length];
        for (int i = 0; i < cars.Length; i++)
            carControllers[i] = cars[i].GetComponent<CarController>();
        ResetGame(0f);
    }

    public void ResetGame(float extraInitialWait)
    {
        // disables all car controllers
        foreach (CarController carController in carControllers)

            carController.enabled = false;

        countdownText = countDown.GetComponent<TMPro.TextMeshProUGUI>();

        StartCoroutine(CountStart(extraInitialWait));

    }

    IEnumerator CountStart(float extraInitialWait)
    {
        yield return new WaitForSeconds(extraInitialWait);
        yield return new WaitForSeconds(0.5f);
        countdownText.color = new Color32(255, 255, 255, 255);
        countDown.SetActive(true);
        countdownText.text = "3";
        getReady.Play();
        countDown.SetActive(true);

        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countdownText.text = "2";
        getReady.Play();
        countDown.SetActive(true);

        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countdownText.text = "1";
        getReady.Play();
        countDown.SetActive(true);

        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countdownText.text = "GO!";
        countdownText.color = new Color32(143, 214, 138, 255);

        goAudio.Play();
        countDown.SetActive(true);

        lapTimer.SetActive(true);

        // enables car movement
        foreach (CarController carController in carControllers)
            carController.enabled = true;
    }
}
