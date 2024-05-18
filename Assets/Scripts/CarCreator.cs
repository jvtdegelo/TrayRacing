using UnityEngine;

public class CarManager : MonoBehaviour
{
    // Array de prefabs de carros
    public GameObject[] carPrefabs;
    // Posição inicial onde o carro será instanciado
    public Transform spawnPoint;
    // Configurações padrão do Particle System
    public ParticleSystem particleSystemPrefab;

    void Start()
    {
        // Seleciona aleatoriamente um carro prefab
        GameObject selectedCarPrefab = carPrefabs[Random.Range(0, carPrefabs.Length)];

        // Instancia o carro selecionado
        GameObject carInstance = Instantiate(selectedCarPrefab, spawnPoint.position, spawnPoint.rotation);

        // Adiciona Particle Systems às rodas
        AddParticleSystemToWheels(carInstance);
    }

    void AddParticleSystemToWheels(GameObject car)
    {
        // Nomes das rodas
        string[] wheelNames = { "wheel_backLeft", "wheel_backRight", "wheel_frontLeft", "wheel_frontRight" };

        foreach (string wheelName in wheelNames)
        {
            // Encontra a roda pelo nome
            Transform wheel = car.transform.Find(wheelName);

            if (wheel != null)
            {
                // Adiciona o componente Particle System à roda
                ParticleSystem particleSystem = Instantiate(particleSystemPrefab, wheel);
                particleSystem.transform.localPosition = Vector3.zero; // Ajusta a posição do Particle System
            }
            else
            {
                Debug.LogError("Roda não encontrada: " + wheelName);
            }
        }
    }
}
