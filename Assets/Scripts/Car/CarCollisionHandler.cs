using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(CarPointer))]
public class CarCollisionHandler : MonoBehaviour
{
    // HashSet para armazenar os objetos que colidiram
    public HashSet<GameObject> allCheckpoints = new HashSet<GameObject>();
    public HashSet<GameObject> collidedCheckpoints = new HashSet<GameObject>();
    public LayerMask wallLayer, checkpointLayer;
    private CarAgent carAgent;
    private CarPointer carPointer;
    private CarController carController;

    public float
        onTriggerPoints = 1,
        onCollisionEnterPoints = -0.5f,
        onCollisionStayPoints = -0.1f;

    private RaycastHit nextCheckpoint;
    private Vector3 nextCheckpointDirection, prevCheckpointDirection;
    private Coroutine checkpointTimer;

    private readonly float checkpointTimerSeconds = 2.5f;

    private void Start()
    {
        if (TryGetComponent(out carPointer))
        {
            carPointer.GetCar().TryGetComponent(out carAgent);
            carPointer.GetCar().TryGetComponent(out carController);
        }

        GameObject map = GameObject.Find("Map");
        if (map != null)
            FindCheckpointsInSelfAndChildren(map);
        else
            Debug.LogError("Could not find GameObject Map in scene");
        // Debug.Log(allCheckpoints.Count);
    }

    public void ResetCheckpoints() { collidedCheckpoints = new HashSet<GameObject>(); }


    void Update()
    {
        // returns to last checkpoint when R is pressed
        if (Input.GetKeyDown(KeyCode.Q))
            Debug.Log(collidedCheckpoints.Count);
    }

    // Método chamado ao colidir com outro objeto (Trigger)
    private void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("triggered with " + collider.gameObject.name);
        if (collider.gameObject.TryGetComponent(out Checkpoint _))
        {
            // Debug.Log("triggered checkpoint");
            bool firstTimeEntering = collidedCheckpoints.Add(collider.gameObject);
            if (firstTimeEntering)
            {
                Transform colliderTransform = collider.gameObject.transform;
                AddPointsToCar(onTriggerPoints);
                FindNextCheckpoint(colliderTransform);
                // sets next checkpoint variables
                carController.SetNextCheckpoint(nextCheckpoint);
                carController.SetNextCheckpointDirection(nextCheckpointDirection);
                // handle checkpoint return position
                Vector3 direction = NormalizeVectorDirection(colliderTransform.forward, prevCheckpointDirection);
                carController.SetCheckpointReturnPosition(colliderTransform.position, Quaternion.LookRotation(direction));
                prevCheckpointDirection = direction;
                HandleCheckpointTimer();
            }
        }
    }

    // Chamado quando a colisão começa
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("collided with " + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out Wall _))
        {
            // Debug.Log("collided on wall");
            AddPointsToCar(onCollisionEnterPoints);
        }
    }


    // Chamado a cada frame enquanto a colisão continua
    void OnCollisionStay(Collision collision)
    {
        // Debug.Log("is staying collided with " + collision.gameObject.name);
        if (collision.gameObject.TryGetComponent(out Wall _))
        {
            // Debug.Log("is staying on wall");
            AddPointsToCar(onCollisionStayPoints);
        }
    }

    private void AddPointsToCar(float pointsToAdd)
    {
        // Debug.Log("adding " + pointsToAdd + " to " + carAgent.gameObject.name);
        if (carAgent != null)
        {
            carAgent.AddReward(pointsToAdd);
            carAgent.AddRewardDebug(pointsToAdd);
            return;
        }
    }

    private void HandleCheckpointTimer()
    {
        if (carAgent == null) return;

        Debug.Log("initializing timer");
        // Resets the timer if it has already started
        if (checkpointTimer != null)
            StopCoroutine(checkpointTimer);


        // Initiate 5 second timer
        checkpointTimer = StartCoroutine(CheckpointTimer());
    }

    private IEnumerator CheckpointTimer()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(checkpointTimerSeconds);
        Debug.Log("checkpointTimer timeout, returning to last checkpoint");

        carController.ReturnToLastCheckpoint();

        // Resets checkpoint timer
        StopCoroutine(checkpointTimer);
        checkpointTimer = StartCoroutine(CheckpointTimer());
    }

    // Método opcional para obter os objetos colididos
    public HashSet<GameObject> GetCollidedCheckpoints()
    {
        return collidedCheckpoints;
    }

    private void FindCheckpointsInSelfAndChildren(GameObject parent)
    {
        if (!parent.activeSelf) return;
        if (parent.TryGetComponent<Checkpoint>(out _))
            allCheckpoints.Add(parent);

        foreach (Transform child in parent.transform)
            FindCheckpointsInSelfAndChildren(child.gameObject);
    }

    private bool IsOnLayer(int layer, LayerMask mask)
    {
        return mask == 1 << layer; // binary shift
    }

    bool HasPassedCheckpoint(Checkpoint checkpoint)
    {
        return collidedCheckpoints.Contains(checkpoint.gameObject);
    }

    private Vector3 NormalizeVectorDirection(Vector3 gameObjForward, Vector3 carForward)
    {
        return Vector3.Normalize(gameObjForward * Vector3.Dot(gameObjForward, carForward));
    }

    public RaycastHit GetFirstCheckpoint(Vector3 direction)
    {
        Vector3 origin = transform.position;
        prevCheckpointDirection = direction;

        float remainingDistance = 40f;
        int i = 0;
        do
        {
            // if finds a checkpoint, save it as nextCheckpoint
            if (Physics.Raycast(origin, direction, out RaycastHit hit, remainingDistance, checkpointLayer))
            {
                if (remainingDistance < 0) break;
                remainingDistance -= hit.distance;
                // move a small distance forward to avoid hitting the same point again
                origin = hit.transform.position + direction * 0.5f;
                // get the direction of the next checkpoint
                // the normalize and dot product is to get relative to the direction the car is facing
                direction = NormalizeVectorDirection(hit.normal, direction);

                nextCheckpoint = hit;
                nextCheckpointDirection = direction;
            }
            else Debug.LogWarning("has not found nextCheckpoint on GetFirstCheckpoint"); ;
            i++;
        } while (i < 3);

        return nextCheckpoint;
    }

    private void FindNextCheckpoint(Transform triggeredCheckpoint)
    {
        Vector3 origin = triggeredCheckpoint.position;
        Vector3 forward = carPointer.GetCar().transform.forward;
        Vector3 direction = NormalizeVectorDirection(triggeredCheckpoint.forward, forward);

        float remainingDistance = 40f;
        int i = 0;
        do
        {
            // if finds a checkpoint, save it as nextCheckpoint
            if (Physics.Raycast(origin, direction, out RaycastHit hit, remainingDistance, checkpointLayer))
            {
                if (remainingDistance < 0) break;
                remainingDistance -= hit.distance;
                // move a small distance forward to avoid hitting the same point again
                origin = hit.transform.position + direction * 0.5f;
                // get the direction of the next checkpoint
                // the normalize and dot product is to get relative to the direction the car is facing
                direction = NormalizeVectorDirection(hit.normal, direction);

                nextCheckpoint = hit;
                nextCheckpointDirection = direction;
            }
            i++;
        } while (i < 5);

    }

}
