using UnityEngine;

public class CarCreatorGiant : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public GameObject selectedCarPrefab;
    public Rigidbody carRigidbodyPrefab;
    public GameObject particleControllerPrefab;
    void Start()
    {
        // if has not selected a car prefab, selects it randomly
        if (selectedCarPrefab == null)
            selectedCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // instanciates the selected car        
        GameObject carInstanceGiant = Instantiate(selectedCarPrefab, transform);

        carInstanceGiant.transform.localPosition = new Vector3(0f, 50f, 0f);
        carInstanceGiant.transform.localScale = new Vector3(18f, 18f, 18f);
        carInstanceGiant.name = "CarModelGiantForMinimap";
    }

    void Update() { }

}
