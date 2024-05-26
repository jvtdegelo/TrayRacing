using UnityEngine;

[RequireComponent(typeof(CarController))]
public class CarAudio : MonoBehaviour
{
    private CarController carController;

    private readonly float
        minPitch = 0.18f,
        maxPitch = 1.0f,
        minSpeed = 0,
        maxSpeed = 4000.0f;

    public AudioSource engine, honk;

    void Start()
    {
        carController = GetComponent<CarController>();

        // TODO: se der tempo refatorar aqui pra adicionar o audio em cada carInstance
        AddAudioSource();
    }

    void Update()
    {
        float speed = Mathf.Abs(carController.speedInput);
        float pitch = Utils.MapRange(speed, minSpeed, maxSpeed, minPitch, maxPitch);
        // Debug.Log(speed + "\t" + pitch);
        engine.pitch = pitch;
        PlayHorn();

        // TODO: alterar vromm (car engine) para que o som seja constante, de modo que o pitch seja bom
        // TODO: se o carro estiver parado e ele acelerar faz som do pneu coisando (pegar velocidade pelo rigidbody)
        // TODO: se o carController estiver desativado (inicio da corrida) e input vertical, fazer o som do motor
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

    private void PlayHorn()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!honk.isPlaying)
                honk.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (honk.isPlaying)
                honk.Stop();
        }
    }
}
