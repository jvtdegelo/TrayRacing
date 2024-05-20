using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointer : MonoBehaviour
{
    private GameObject car;

    public void SetCar(GameObject car)
    {
        this.car = car;
    }
    public GameObject GetCar()
    {
        return this.car;
    }

    void Start() { }

    void Update() { }
}
