using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownController : MonoBehaviour {

    public GameObject countDown;
    public AudioSource getReady;
    public AudioSource goAudio;
    public GameObject lapTimer;
    public GameObject car;
    private CarController carController;

    void Start() {
        StartCoroutine(CountStart());
    }

    IEnumerator CountStart() {
        yield return new WaitForSeconds(0.5f);
        countDown.GetComponent<TMPro.TextMeshProUGUI>().text = "3";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countDown.GetComponent<TMPro.TextMeshProUGUI>().text = "2";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countDown.GetComponent<TMPro.TextMeshProUGUI>().text = "1";
        getReady.Play();
        countDown.SetActive(true);
        yield return new WaitForSeconds(1);
        countDown.SetActive(false);
        countDown.GetComponent<TMPro.TextMeshProUGUI>().text = "GO!";
        countDown.GetComponent<TMPro.TextMeshProUGUI>().color = new Color32(143,214,138,255);

        goAudio.Play();
        countDown.SetActive(true);

        lapTimer.SetActive(true);        
        carController = car.GetComponent<CarController>();
        carController.enabled = true;

    }
}
