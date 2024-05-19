using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody carRigidbody;

    public float forwardAcceleration = 8f;
    public float reverseAcceleration = 4f;
    public float maximumSpeed = 50f;
    public float turnStrength = 180f;
    public float dragOnGround = 3f;

    public float incrementGravityForce = 10f;

    private float speedInput;

    private bool isOnGround, isOnDirt;

    public LayerMask whatIsGround;
    public LayerMask whatIsDirt;
    public float groundRayLength = .5f;
    public Transform groundRayPoint;

    public Transform frontLeftWheel, frontRightWheel;
    public float maxTurnWheel = 25f;
    public float maxEmission = 25f;


    public void SetFrontLeftWheel(Transform frontLeftWheel)
    {
        this.frontLeftWheel = frontLeftWheel;
    }
    public void SetFrontRightWheel(Transform frontRightWheel)
    {
        this.frontRightWheel = frontRightWheel;
    }

    public Rigidbody GetCarRigidbody()
    {
        return this.carRigidbody;
    }

    void Start()
    {
        // prevent "double-movement" of sphere and car (parent)
        carRigidbody.transform.parent = null;
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

        if (isOnGround || isOnDirt)
        {
            // rotates, on the y axis, when is moving (verticalInput) and turning (horizontalInput)
            // the signal for the rotation is giver both by the horizontalInput and the verticalInput
            Vector3 rotationDiff = new Vector3(0f, horizontalInput * turnStrength * Time.deltaTime * verticalInput, 0f);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotationDiff);
        }


        // Example of using the front wheels in the update method
        if (frontRightWheel != null && frontLeftWheel != null)
        {
            // get the front wheel angles
            Vector3 leftWheelAngles = frontLeftWheel.localRotation.eulerAngles;
            Vector3 rightWheelAngles = frontRightWheel.localRotation.eulerAngles;

            // rotate them on the y axis based on the horizontal input
            frontLeftWheel.localRotation = Quaternion.Euler(leftWheelAngles.x, (horizontalInput * maxTurnWheel) - 180, leftWheelAngles.z);
            frontRightWheel.localRotation = Quaternion.Euler(rightWheelAngles.x, horizontalInput * maxTurnWheel, rightWheelAngles.z);
        }

        transform.position = carRigidbody.transform.position;
    }

    private void FixedUpdate()
    {
        RaycastHit hit, hitGround, hitDirt;

        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        isOnGround = Physics.Raycast(groundRayPoint.position, -transform.up, out hitGround, groundRayLength, whatIsGround);
        isOnDirt = Physics.Raycast(groundRayPoint.position, -transform.up, out hitDirt, groundRayLength, whatIsDirt);

        if (isOnGround || isOnDirt)
        {
            hit = hitGround.collider != null ? hitGround : hitDirt;
            // rotates the car upwards/downwards if it is moving on a slope
            // TODO: verificar se, usando a logica do left/right wheel, fica mais suave
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            carRigidbody.drag = dragOnGround;
            // accelerates the car
            if (Mathf.Abs(speedInput) > 0)
            {
                carRigidbody.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            // has less drag when on air
            carRigidbody.drag = 0.1f;
            // adds extra gravitational force for more realism
            carRigidbody.AddForce(incrementGravityForce * 100f * Vector3.down);
        }
    }
}
