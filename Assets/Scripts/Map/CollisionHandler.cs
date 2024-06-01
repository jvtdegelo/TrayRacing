using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshCollider))]
public class CollisionHandler : MonoBehaviour
{
    // HashSet para armazenar os objetos que colidiram
    public HashSet<GameObject> collidedObjects = new HashSet<GameObject>();
    public float
        onTriggerPoints = 1,
        onCollisionEnterPoints = -0.5f,
        onCollisionStayPoints = -0.1f;

    private void Awake()
    {
        Transform checkpoints = transform.Find("Checkpoints");
    }

    // Método chamado ao colidir com outro objeto (Trigger)
    private void OnTriggerEnter(Collider collider)
    {
        // Debug.Log(collider.gameObject.name + " triggered " + transform.parent.name);
        bool firstTimeEntering = collidedObjects.Add(collider.gameObject);
        if (firstTimeEntering)
            AddPointsToCar(collider.gameObject, onTriggerPoints);
    }

    // Chamado quando a colisão começa
    void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name + " collided with " + transform.parent.name);
        AddPointsToCar(collision.gameObject, onCollisionEnterPoints);
    }


    // Chamado a cada frame enquanto a colisão continua
    void OnCollisionStay(Collision collision)
    {
        // Debug.Log(collision.gameObject.name + " is still colliding with " + transform.parent.name);
        AddPointsToCar(collision.gameObject, onCollisionStayPoints);
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
    public HashSet<GameObject> GetCollidedObjects()
    {
        return collidedObjects;
    }
}
