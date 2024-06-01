using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;

[RequireComponent(typeof(CarController))]
public class CarCollisionHandler : MonoBehaviour
{
    // HashSet para armazenar os objetos que colidiram
    public GameObject checkpointContainer;
    public HashSet<GameObject> allCheckpoints = new HashSet<GameObject>();
    public HashSet<GameObject> collidedCheckpoints = new HashSet<GameObject>();
    public LayerMask Wall, Checkpoint;

    private CarAgent carAgent;
    public float
        onTriggerPoints = 1,
        onCollisionEnterPoints = -0.5f,
        onCollisionStayPoints = -0.1f;

    private void Awake()
    {
        TryGetComponent(out carAgent);
        FindCheckpointsInSelfAndChildren(checkpointContainer);
        Debug.Log(allCheckpoints.Count);
    }

    // Método chamado ao colidir com outro objeto (Trigger)
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.gameObject.name + " triggered " + transform.parent.name);
        Debug.Log(collider.gameObject.layer + " , " + Checkpoint.value);
        if (IsOnLayer(collider.gameObject.layer, Checkpoint))
        {
            bool firstTimeEntering = collidedCheckpoints.Add(collider.gameObject);
            if (firstTimeEntering)
                AddPointsToCar(collider.gameObject, onTriggerPoints);
        }
    }

    // Chamado quando a colisão começa
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " collided with " + transform.parent.name);
        Debug.Log(collision.gameObject.layer + " , " + Wall.value);
        if (IsOnLayer(collision.gameObject.layer, Wall))
        {
            AddPointsToCar(collision.gameObject, onCollisionEnterPoints);
        }
    }


    // Chamado a cada frame enquanto a colisão continua
    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name + " is still colliding with " + transform.parent.name);
        Debug.Log(collision.gameObject.layer + " , " + Wall.value);
        if (IsOnLayer(collision.gameObject.layer, Wall))
        {
            AddPointsToCar(collision.gameObject, onCollisionStayPoints);
        }
    }

    private void AddPointsToCar(GameObject carRigidbody, float pointsToAdd)
    {
        if (carRigidbody.TryGetComponent(out CarPointer carPointer))
        {
            if (carPointer.GetCar().TryGetComponent(out CarAgent carAgent))
            {
                carAgent.AddReward(pointsToAdd);
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
        if (IsOnLayer(parent.layer, Checkpoint))
            allCheckpoints.Add(parent);

        foreach (Transform child in parent.transform)
            FindCheckpointsInSelfAndChildren(child.gameObject);
    }

    private bool IsOnLayer(int layer, LayerMask mask)
    {
        return mask == 1 << layer; // binary shift
    }
}
