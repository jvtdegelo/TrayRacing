using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody sphere;

    public float forwardAcceleration    = 8f;
    public float reverseAcceleration    = 4f;
    public float maximumSpeed           = 50f;
    public float turnStrength           = 180f;
    public float dragOnGround           = 3f;

    public float incrementGravityForce = 10f;
    
    private float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength=.5f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxTurnWheel=25f;

    public ParticleSystem[] dustTrail;
    public float maxEmission = 25f;
    private float emissionRate;

    void Start()
    {
        sphere.transform.parent = null;
    }

    void Update()
    {
        speedInput = 0f;
        var verticalInput   = Input.GetAxis("Vertical");
        var horizontalInput = Input.GetAxis("Horizontal");
        if(verticalInput>0)
            speedInput = verticalInput * forwardAcceleration * 1000f;
        else
            speedInput = verticalInput * reverseAcceleration * 1000f;
            
        if(grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, horizontalInput * turnStrength* Time.deltaTime * verticalInput, 0f));
        }
        
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (horizontalInput*maxTurnWheel) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, horizontalInput*maxTurnWheel, rightFrontWheel.localRotation.eulerAngles.z);
        
        transform.position = sphere.transform.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if(Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        emissionRate = 0;

        if(grounded)
        {
            sphere.drag = dragOnGround;
            if(Mathf.Abs(speedInput)>0)
            {
                sphere.AddForce(transform.forward * speedInput);
                emissionRate = maxEmission;
            }
        } else 
        {
            sphere.drag = 0.1f;
            sphere.AddForce(Vector3.up * -incrementGravityForce * 100f);
        }
        foreach(ParticleSystem part in dustTrail)
        {
            var emissionModule = part.emission;
            emissionModule.rateOverTime = emissionRate;
        }
    }
}
