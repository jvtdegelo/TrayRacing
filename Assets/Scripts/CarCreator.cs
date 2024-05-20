using UnityEngine;

public class CarCreator : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public GameObject selectedCarPrefab;
    public Rigidbody carRigidbodyPrefab;
    public GameObject particleControllerPrefab;
    private CarController carController;

    void Start()
    {
        // if has not selected a car prefab, selects it randomly
        if (selectedCarPrefab == null)
            selectedCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // instanciates the selected car
        GameObject carInstance = Instantiate(selectedCarPrefab, transform);
        carInstance.transform.localPosition = new Vector3(0f, -1.0f, 0f);
        carInstance.name = "CarModel";
        carController = this.GetComponent<CarController>();

        if (carController != null)
            carController.SetCarRigidbody(Instantiate(carRigidbodyPrefab, transform));

        AddParticleControllerToWheels(carInstance);
    }

    void Update() { }

    private void AddParticleControllerToWheels(GameObject car)
    {
        // names of the wheel sprites (in all cars)
        string[] wheelNames = { "wheel_backLeft", "wheel_backRight", "wheel_frontLeft", "wheel_frontRight" };

        if (carController == null)
        {
            Debug.LogError("Could not find CarController on Component " + this.name);
            return;
        }

        foreach (string wheelName in wheelNames)
        {
            // finds the wheel by name
            Transform wheel = car.transform.Find(wheelName);

            if (wheel == null) Debug.LogError("Could not find " + wheelName);
            else
            {
                // adds the particle controller to the wheels
                GameObject particleControllerInstance = Instantiate(particleControllerPrefab, wheel);

                // adds the car rigidbody to the particle controller (required)
                ParticleController particleController = particleControllerInstance.GetComponent<ParticleController>();
                particleController.SetCarRigidbody(carController.GetCarRigidbody());

                // sets the front wheels to the car controller (for it to steer them)
                if (wheelName == "wheel_frontLeft") carController.SetFrontLeftWheel(wheel);
                if (wheelName == "wheel_frontRight") carController.SetFrontRightWheel(wheel);
            }
        }
    }
}
