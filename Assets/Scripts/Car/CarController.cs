using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody carRigidbody;
    private GameObject carModel;

    // TODO: maximumSpeed is not used
    public float maximumSpeed = 50f, forwardAcceleration = 7f, reverseAcceleration = 4f;
    public float maxSpeedGround = 50f, accelerationGround = 7f, reverseAccelerationGround = 4f;
    public float maxSpeedContact = 20f, accelerationContact = 4f, reverseAccelerationContact = 2f;
    public float maxSpeedDirt = 10f, accelerationDirt = 4f, reverseAccelerationDirt = 2f;
    public float turnStrength = 180f;
    public float dragOnGround = 3f, dragOnAir = 0.2f;
    public float incrementGravityForce = 10f;

    public float speedInput;

    private bool isOnGround, isOnDirt, isOnContact;

    public LayerMask GroundLayer, DirtLayer, CheckpointLayer;
    public float groundRayLength = 1f;
    private Transform groundRay;

    private Transform frontLeftWheel, frontRightWheel;
    public float maxTurnWheel = 25f;

    private float verticalInput = 0f, horizontalInput = 0f;


    private Vector3 lastCheckpointPosition, spawnPosition;
    private Quaternion lastCheckpointRotation, spawnRotation;

    private RaycastHit nextCheckpoint;
    private CarCollisionHandler carCollisionHandler;

    public void SetCarModel(GameObject carModel) { this.carModel = carModel; }
    public void SetFrontLeftWheel(Transform frontLeftWheel) { this.frontLeftWheel = frontLeftWheel; }
    public void SetFrontRightWheel(Transform frontRightWheel) { this.frontRightWheel = frontRightWheel; }
    public void SetIsOnContact(bool isOnContact) { this.isOnContact = isOnContact; }

    public Rigidbody GetCarRigidbody() { return carRigidbody; }
    public void SetCarRigidbody(Rigidbody carRigidbody)
    {
        this.carRigidbody = carRigidbody;
        carRigidbody.name = name + " Rigidbody";
        carRigidbody.transform.parent = null;
        // adds the carPointer to the rigidbody
        if (carRigidbody.TryGetComponent(out CarPointer carPointer))
            carPointer.SetCar(gameObject);
        carRigidbody.TryGetComponent(out carCollisionHandler);
    }
    public void SetInputs(float verticalInput = 0f, float horizontalInput = 0f)
    {
        this.verticalInput = verticalInput;
        this.horizontalInput = horizontalInput;
    }

    private void SetNextCheckpoint(RaycastHit nextCheckpoint) { this.nextCheckpoint = nextCheckpoint; }
    public RaycastHit GetNextCheckpoint() { return nextCheckpoint; }

    public void ResetCheckpoints()
    {
        if (carRigidbody.TryGetComponent(out carCollisionHandler))
            carCollisionHandler.ResetCheckpoints();
    }

    void Start()
    {
        lastCheckpointPosition = transform.position;
        lastCheckpointRotation = transform.rotation;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        InitializeGroundRayTransform();
    }

    void Update()
    {
        speedInput = 0f;

        // accelerates or reverses based on verticalInput
        // TODO: talvez trocar para que os inputs sejam sempre 0, 1, e -1, e fazer com que ele acelere suavemente por um slerp?
        if (verticalInput > 0)
            speedInput = verticalInput * forwardAcceleration * 1000f;
        else
            speedInput = verticalInput * reverseAcceleration * 1000f;

        RotateCarOnSurface(horizontalInput, verticalInput);

        SteerWheels(horizontalInput);

        transform.position = carRigidbody.transform.position;
    }
    private void FixedUpdate()
    {
        // the car is on the ground if a ray, starting on groundRayPoint, going downward for its length, hits the ground
        isOnGround = Physics.Raycast(groundRay.position, -transform.up, out RaycastHit hitGround, groundRayLength, GroundLayer);
        isOnDirt = Physics.Raycast(groundRay.position, -transform.up, out RaycastHit hitDirt, groundRayLength, DirtLayer);

        RaycastHit? nextCheckpoint = carCollisionHandler.GetNextCheckpoint(transform.forward);
        if (nextCheckpoint != null)
            SetNextCheckpoint((RaycastHit)nextCheckpoint);

        UpdateSpeedAcceleration();

        if (isOnGround || isOnDirt || isOnContact)
        {
            RaycastHit hit = hitGround.collider != null ? hitGround : hitDirt;
            RotateCarModelOnNormal(hit.normal);

            carRigidbody.drag = dragOnGround;
            // accelerates the car
            if (Mathf.Abs(speedInput) > 0)
                carRigidbody.AddForce(transform.forward * speedInput);
        }
        else
        {
            carRigidbody.drag = dragOnAir;
            // adds extra gravitational force for more realism
            carRigidbody.AddForce(incrementGravityForce * 120f * Vector3.down);
        }
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

    private void RotateCarOnSurface(float horizontalInput, float verticalInput)
    {
        // rotates, on the y axis, when is moving (verticalInput) and turning (horizontalInput)
        // the sign of the rotation is given both by the horizontalInput and the verticalInput
        if (isOnGround || isOnDirt || isOnContact)
        {
            float rotationStrength = turnStrength * horizontalInput * verticalInput * Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, rotationStrength, 0f));
        }
    }

    private void SteerWheels(float horizontalInput)
    {
        if (frontRightWheel == null || frontLeftWheel == null)
            return;
        // TODO: add rotation on x axis based on speed
        // get the front wheel angles
        Vector3 leftWheelAngles = frontLeftWheel.localRotation.eulerAngles;
        Vector3 rightWheelAngles = frontRightWheel.localRotation.eulerAngles;
        // rotate them on the y axis based on the horizontal input
        frontLeftWheel.localRotation = Quaternion.Euler(leftWheelAngles.x, (horizontalInput * maxTurnWheel) - 180, leftWheelAngles.z);
        frontRightWheel.localRotation = Quaternion.Euler(rightWheelAngles.x, horizontalInput * maxTurnWheel, rightWheelAngles.z);
    }

    // rotates the car upwards/downwards if it is moving on a slope
    private void RotateCarModelOnNormal(Vector3 surfaceNormal)
    {
        // calculates desired rotation based on surfaceNormal
        Quaternion targetRotation = Quaternion.FromToRotation(carModel.transform.up, surfaceNormal) * carModel.transform.rotation;
        // smoothes the rotation using slerp
        carModel.transform.rotation = Quaternion.Slerp(carModel.transform.rotation, targetRotation, 0.1f);
    }

    public void SetLastCheckpointPosition(Vector3 position, Quaternion rotation)
    {
        lastCheckpointPosition = position + new Vector3(0f, 3f, 0f); // spawns on air
        lastCheckpointRotation = rotation;
    }
    public void ReturnToLastCheckpoint()
    {
        carRigidbody.position = lastCheckpointPosition;
        transform.rotation = lastCheckpointRotation;
        StopCompletely();
    }
    public void ReturnToSpawn()
    {
        carRigidbody.position = spawnPosition;
        transform.rotation = spawnRotation;
        StopCompletely();
    }

    public void StopCompletely()
    {
        carRigidbody.velocity = Vector3.zero;
        carRigidbody.angularVelocity = Vector3.zero;
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
        else if (isOnContact)
        {
            maximumSpeed = maxSpeedContact;
            forwardAcceleration = accelerationContact;
            reverseAcceleration = reverseAccelerationContact;
        }
    }
}
