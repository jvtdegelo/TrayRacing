using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPointer : MonoBehaviour
{
    private GameObject car;
    private CarController carController;

    public void SetCar(GameObject car)
    {
        this.car = car;
        car.TryGetComponent(out carController);
    }
    public GameObject GetCar() { return this.car; }

    void Start() { }
    void Update() { }

    void OnCollisionEnter(Collision other) { carController.SetIsOnContact(true); }
    void OnCollisionExit() { carController.SetIsOnContact(false); }

}
