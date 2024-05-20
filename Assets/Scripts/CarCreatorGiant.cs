using UnityEngine;

public class CarCreatorGiant : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public GameObject selectedCarPrefab;
    public Rigidbody carRigidbodyPrefab;
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
        removeShadows(carInstanceGiant);
    }

    private void removeShadows(GameObject car) {
        Renderer[] renderers = car.GetComponentsInChildren<Renderer>();

        // Iterate over all Renderer components
        foreach (Renderer renderer in renderers)
        {
            Debug.Log("Renderer found in: " + renderer.gameObject.name);
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }       

     }

}
