using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody carRigidbody;

    public float maximumSpeed = 50f, forwardAcceleration = 8f, reverseAcceleration = 4f;
    public float maxSpeedGround = 50f, accelerationGround = 8f, reverseAccelerationGround = 4f;
    public float maxSpeedDirt = 10f, accelerationDirt = 2f, reverseAccelerationDirt = 1f;
    public float turnStrength = 180f;
    public float dragOnGround = 3f;

    public float incrementGravityForce = 10f;

    private float speedInput;

    private bool isOnGround, isOnDirt;

    public LayerMask GroundLayer;
    public LayerMask DirtLayer;
    public float groundRayLength = 1f;
    private Transform groundRay;

    public Transform frontLeftWheel, frontRightWheel;
    public float maxTurnWheel = 25f;

    private int rewardPoints = 0;


    public void SetFrontLeftWheel(Transform frontLeftWheel)
    {
        this.frontLeftWheel = frontLeftWheel;
    }
    public void SetFrontRightWheel(Transform frontRightWheel)
    {
        this.frontRightWheel = frontRightWheel;
    }

    public void AddRewardPoints(int pointsToAdd)
    {
        this.rewardPoints += pointsToAdd;
        Debug.Log(gameObject.name + " points: " + rewardPoints);
    }

    public Rigidbody GetCarRigidbody()
    {
        return this.carRigidbody;
    }
    public void SetCarRigidbody(Rigidbody carRigidbody)
    {
        this.carRigidbody = carRigidbody;
        carRigidbody.name = this.name + " Rigidbody";
        carRigidbody.transform.parent = null;
        // adds the carPointer to the rigidbody
        CarPointer carPointer;
        if (carRigidbody.TryGetComponent(out carPointer))
            carPointer.SetCar(this.gameObject);
    }

    void Start()
    {
        // prevent "double-movement" of sphere and car (parent)
        InitializeGroundRayTransform();
    }

    private void InitializeGroundRayTransform()
    {
        // Criar um novo GameObject
        GameObject groundRayObj = new GameObject("Car RayPoint");
        groundRayObj.transform.parent = transform;

        // Acessar o Transform do GameObject
        Transform groundRay = groundRayObj.transform;

        // Inicializar a posição, rotação e escala
        groundRay.SetLocalPositionAndRotation(new Vector3(0, -0.95f, 0), Quaternion.identity);

        this.groundRay = groundRay;
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

    private void UpdateSpeedAcceleration()
    {
        if (isOnGround)
        {
            maximumSpeed = maxSpeedGround;
            forwardAcceleration = accelerationGround;
            reverseAcceleration = reverseAccelerationGround;
        }
        else if (isOnDirt)
        {
            maximumSpeed = maxSpeedDirt;
            forwardAcceleration = accelerationDirt;
            reverseAcceleration = reverseAccelerationDirt;
        }
    }

    private void FixedUpdate()
    {
        RaycastHit hit, hitGround, hitDirt;

        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        isOnGround = Physics.Raycast(groundRay.position, -transform.up, out hitGround, groundRayLength, GroundLayer);
        isOnDirt = Physics.Raycast(groundRay.position, -transform.up, out hitDirt, groundRayLength, DirtLayer);

        UpdateSpeedAcceleration();

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
            carRigidbody.drag = 0.2f;
            // adds extra gravitational force for more realism
            carRigidbody.AddForce(incrementGravityForce * 120f * Vector3.down);
        }
    }
}
