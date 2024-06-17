using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class ParticleController : MonoBehaviour
{

    public float maximumSpeed = 50f;
    public Color
        groundColor = new Color(0.8f, 0.1f, 0.1f), // Cinza
        dirtColor = new Color(0.96f, 0.87f, 0.70f), // Bege
        groundTrailColor = new Color(0, 0, 0),
        dirtTrailColor = new Color(0.96f, 0.87f, 0.70f);
    public LayerMask GroundLayer, DirtLayer;
    public Transform rayPoint;
    public float rayLength = .5f;
    private float emissionRate, maxEmission = 25f;
    public ParticleSystem particle;
    public TrailRenderer trailRenderer;
    private Rigidbody carRigidbody;
    private CarController carController;

    void Start() { }

    void Update() { }

    public void SetCarRigidbody(Rigidbody carRigidbody) { this.carRigidbody = carRigidbody; }
    public void SetCarController(CarController carController) { this.carController = carController; }

    private void FixedUpdate()
    {
        emissionRate = 0;

        float speed = carRigidbody.velocity.magnitude;

        // the car is on a terrain X if a ray, starting on groundRayPoint, going downward for its length, hits the terrain
        bool isOnGround = Physics.Raycast(rayPoint.position, -transform.up, out _, rayLength, GroundLayer);
        bool isOnDirt = Physics.Raycast(rayPoint.position, -transform.up, out _, rayLength, DirtLayer);

        // add particle emission if car is moving and on ground
        if ((isOnGround || isOnDirt) && speed > 1)
        {
            emissionRate = Utils.MapRange(speed, 0, maximumSpeed, 0, maxEmission);
        }

        // defines the color based on the terrain
        if (isOnGround)
            SetParticleColor(groundColor);
        else if (isOnDirt)
            SetParticleColor(dirtColor);

        SetEmissionRate(emissionRate);

        if (carController.isTurning)
        {
            trailRenderer.emitting = true;
            // trailRenderer.material.color = isOnGround ? groundTrailColor : dirtTrailColor;
            // Color trailColor = isOnGround ? groundTrailColor : dirtTrailColor;
            // trailRenderer.startColor = trailColor;
            // trailRenderer.endColor = trailColor;//isOnGround ? groundTrailColor : dirtTrailColor;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }

    /// <summary> Set the color of the ParticleSystem </summary>
    void SetParticleColor(Color color)
    {
        var main = particle.main;
        main.startColor = color;
    }

    /// <summary> Set the emission rate of the ParticleSystem </summary>
    void SetEmissionRate(float emissionRate)
    {
        var emissionModule = particle.emission;
        emissionModule.rateOverTime = emissionRate;
    }
}
