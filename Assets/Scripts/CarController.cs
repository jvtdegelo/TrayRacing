using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphere;

    public float forwardAcceleration = 8f;
    public float reverseAcceleration = 4f;
    public float maximumSpeed = 50f;
    public float turnStrength = 180f;
    public float dragOnGround = 3f;

    public float incrementGravityForce = 10f;

    private float speedInput;

    private bool isOnGround;

    public LayerMask whatIsGround;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxTurnWheel = 25f;

    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f;
    private float emissionRate;

    void Start()
    {
        // prevent "double-movement" of sphere and car (parent)
        sphere.transform.parent = null;
    }

    void Update()
    {
        speedInput = 0f;
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // accelerates or reverses based on verticalInput
        if (verticalInput > 0)
            speedInput = verticalInput * forwardAcceleration * 1000f;
        else
            speedInput = verticalInput * reverseAcceleration * 1000f;

        if (isOnGround)
        {
            // rotates, on the y axis, when is moving (verticalInput) and turning (horizontalInput)
            // the signal for the rotation is giver both by the horizontalInput and the verticalInput
            Vector3 rotationDiff = new Vector3(0f, horizontalInput * turnStrength * Time.deltaTime * verticalInput, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationDiff);
        }

        // get the front wheel angles
        Vector3 leftWheelAngles = leftFrontWheel.localRotation.eulerAngles;
        Vector3 rightWheelAngles = rightFrontWheel.localRotation.eulerAngles;

        // rotate them on the y axis based on the horizontal input
        leftFrontWheel.localRotation = Quaternion.Euler(leftWheelAngles.x, (horizontalInput * maxTurnWheel) - 180, leftWheelAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightWheelAngles.x, horizontalInput * maxTurnWheel, rightWheelAngles.z);

        transform.position = sphere.transform.position;
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        emissionRate = 0;

        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        isOnGround = Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround);

        if (isOnGround)
        {
            // rotates the car upwards/downwards if it is moving on a slope
            // TODO: verificar se, usando a logica do left/right wheel, fica mais suave
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            sphere.drag = dragOnGround;
            // accelerates the car, and add particle emission
            if (Mathf.Abs(speedInput) > 0)
            {
                sphere.AddForce(transform.forward * speedInput);
                // TODO: emissionRate baseado em velocidade, e maior quando fazendo curva?
                emissionRate = maxEmission;
            }
        }
        else
        {
            sphere.drag = 0.1f; // has less drag when on air
            // adds extra gravitational force for more realism
            sphere.AddForce(-incrementGravityForce * 100f * Vector3.up);
        }

        foreach (ParticleSystem part in dustTrail)
        {
            var emissionModule = part.emission;
            emissionModule.rateOverTime = emissionRate;
        }
    }
}
