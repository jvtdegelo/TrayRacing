using UnityEngine;

[RequireComponent(typeof(CarController))]
public class AICarAudio : MonoBehaviour
{
    private CarController carController;

    private readonly float
        minPitch = 0.18f,
        maxPitch = 1.0f,
        minSpeed = 0,
        maxSpeed = 4000.0f;

    public AudioSource engine;

    void Start()
    {
        carController = GetComponent<CarController>();

        AddAudioSource();
    }

    void Update()
    {
        float speed = Mathf.Abs(carController.speedInput);
        float pitch = Utils.MapRange(speed, minSpeed, maxSpeed, minPitch, maxPitch);
        engine.pitch = pitch;
    }

    private void AddAudioSource()
    {
        if (engine == null)
        {
            Debug.LogError("Could not find AudioSource on Component " + this.name);
            return;
        }
        engine.pitch = minPitch;
    }

}
