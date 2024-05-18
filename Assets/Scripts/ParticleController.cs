using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    public float maximumSpeed = 50f;
    private Color dirtColor = new Color(0.4f, 0.2f, 0.1f); // Marrom
    private Color sandColor = new Color(0.96f, 0.87f, 0.70f); // Bege
    public LayerMask whatIsGround;
    public LayerMask whatIsDirt;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;
    public float maxEmission = 25f;
    private float emissionRate;
    public ParticleSystem particle;

    public Rigidbody carRigidbody;

    void Start()
    {
        // prevent "double-movement" of sphere and car (parent)
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        emissionRate = 0;

        float speed = carRigidbody.velocity.magnitude;

        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        bool isOnGround = Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround);

        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        bool isOnDirt = Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsDirt);

        // add particle emission if car is moving and on ground
        if ((isOnGround || isOnDirt) && speed > 0)
        {
            // TODO: emissionRate baseado em velocidade, e maior quando fazendo curva?
            emissionRate = maxEmission;
        }
        // Verifica a tag ou material do terreno atingido
        // if (hit.collider.gameObject.layer == whatIsGround)
        if (isOnGround)
        {
            SetParticleColor(particle, dirtColor);
        }
        // else if (hit.collider.gameObject.layer == whatIsDirt)
        else if (isOnDirt)
        {
            SetParticleColor(particle, sandColor);
        }

        var emissionModule = particle.emission;
        emissionModule.rateOverTime = emissionRate;
    }


    void SetParticleColor(ParticleSystem particleSystem, Color color)
    {
        // Modificar a cor das partículas
        var main = particleSystem.main;
        main.startColor = color;
    }
}
