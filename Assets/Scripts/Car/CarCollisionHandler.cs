using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

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
    private Vector3 nextCheckpointDirection;

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
                AddPointsToCar(onTriggerPoints);
                FindNextCheckpoint(collider.gameObject.transform);
                carController.SetNextCheckpoint(nextCheckpoint);
                carController.SetNextCheckpointDirection(nextCheckpointDirection);
                // Debug.Log(nextCheckpoint.collider.transform.position + " " + nextCheckpointDirection);
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
        // Debug.Log("adding " + pointsToAdd + " to " + transform.name);
        if (carAgent != null)
        {
            // Debug.Log("adding to existing carAgent");
            carAgent.AddReward(pointsToAdd);
            carAgent.AddRewardDebug(pointsToAdd);
            return;
        }

        // // Debug.Log("getting new carPointer");
        // if (TryGetComponent(out carPointer))
        // {
        //     // Debug.Log("getting new carAgent");
        //     if (carPointer.GetCar().TryGetComponent(out carAgent))
        //     {
        //         // Debug.Log("adding to new carAgent");
        //         carAgent.AddReward(pointsToAdd);
        //         carAgent.AddRewardDebug(pointsToAdd);
        //     }
        // }
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

    public RaycastHit GetFirstCheckpoint(Vector3 direction)
    {
        Vector3 origin = transform.position;
        nextCheckpointDirection = direction;

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
                direction = Vector3.Normalize(hit.normal * Vector3.Dot(hit.normal, direction));

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
        Vector3 direction = Vector3.Normalize(triggeredCheckpoint.forward * Vector3.Dot(triggeredCheckpoint.forward, forward));

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
                direction = Vector3.Normalize(hit.normal * Vector3.Dot(hit.normal, direction));

                nextCheckpoint = hit;
                nextCheckpointDirection = direction;
            }
            i++;
        } while (i < 5);

    }

}
