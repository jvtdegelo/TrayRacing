using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshCollider))]
public class CollisionLogger : MonoBehaviour
{
    // HashSet para armazenar os objetos que colidiram
    public HashSet<GameObject> collidedObjects = new HashSet<GameObject>();

    // Método chamado ao iniciar
    void Start() { }

    // Método chamado ao colidir com outro objeto (Trigger)
    void OnTriggerEnter(Collider other)
    {
        LogCollision(other.gameObject);
    }

    private void LogCollision(GameObject collidedObject)
    {
        Debug.Log(this.transform.parent.name + " collided with " + collidedObject.name);
        // add collidedObject to set
        bool firstTimeEntering = collidedObjects.Add(collidedObject);
        CarPointer carPointer = collidedObject.GetComponent<CarPointer>();
        // add points if it's the first time entering
        if (firstTimeEntering && carPointer != null)
        {
            // gets the car
            GameObject car = carPointer.GetCar();
            // adds points
            CarController carController;
            if (car.TryGetComponent(out carController))
                carController.AddRewardPoints(1);
        }
    }

    // Método opcional para obter os objetos colididos
    public HashSet<GameObject> GetCollidedObjects()
    {
        return collidedObjects;
    }
}
