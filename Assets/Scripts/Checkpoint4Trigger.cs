using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint4Trigger : MonoBehaviour {
    public GameObject checkpoint1Trig;
    public GameObject checkpoint2Trig;
    public GameObject checkpoint3Trig;
    public GameObject checkpoint4Trig;

    public LapTimeController lapTimeController;

    void OnTriggerEnter () {

        checkpoint1Trig.SetActive (true);
        checkpoint2Trig.SetActive (false);
        checkpoint3Trig.SetActive (false);
        checkpoint4Trig.SetActive (false);
    }
}
