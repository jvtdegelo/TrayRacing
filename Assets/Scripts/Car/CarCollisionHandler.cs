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

    public float
        onTriggerPoints = 1,
        onCollisionEnterPoints = -0.5f,
        onCollisionStayPoints = -0.1f;

    private void Start()
    {
        if (TryGetComponent(out carPointer))
            carPointer.GetCar().TryGetComponent(out carAgent);

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
        if (collider.gameObject.TryGetComponent(out Checkpoint _))
        {
            // Debug.Log(collider.gameObject.name + " triggered " + transform.name);
            bool firstTimeEntering = collidedCheckpoints.Add(collider.gameObject);
            if (firstTimeEntering)
                AddPointsToCar(onTriggerPoints);
        }
    }

    // Chamado quando a colisão começa
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Wall _))
        {
            // Debug.Log(collision.gameObject.name + " collided with " + transform.name);
            AddPointsToCar(onCollisionEnterPoints);
        }
    }


    // Chamado a cada frame enquanto a colisão continua
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Wall _))
        {
            // Debug.Log(collision.gameObject.name + " is still colliding with " + transform.name);
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

        // Debug.Log("getting new carPointer");
        if (TryGetComponent(out carPointer))
        {
            // Debug.Log("getting new carAgent");
            if (carPointer.GetCar().TryGetComponent(out carAgent))
            {
                // Debug.Log("adding to new carAgent");
                carAgent.AddReward(pointsToAdd);
                carAgent.AddRewardDebug(pointsToAdd);
            }
        }
    }

    // Método opcional para obter os objetos colididos
    public HashSet<GameObject> GetCollidedCheckpoints()
    {
        return collidedCheckpoints;
    }

    private void FindCheckpointsInSelfAndChildren(GameObject parent)
    {
        if (IsOnLayer(parent.layer, checkpointLayer))
            allCheckpoints.Add(parent);

        foreach (Transform child in parent.transform)
            FindCheckpointsInSelfAndChildren(child.gameObject);
    }

    private bool IsOnLayer(int layer, LayerMask mask)
    {
        return mask == 1 << layer; // binary shift
    }
}
