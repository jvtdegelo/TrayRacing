using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private readonly float
        Midnight = 0f,
        Dawn = 6f,
        Midday = 12f,
        Dusk = 18f;

    private readonly float
        HoursPerSecond = 0.2f, // how many hours of a day passes per second of gameplay
        FogMin = 0f,
        FogMax = 0.05f;

    private void Start()
    {
        float[] TimesArray = { Midnight, Dawn, Midday, Dusk };
        TimeOfDay = TimesArray[Random.Range(0, TimesArray.Length)];
    }

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            //(Replace with a reference to the game time)
            TimeOfDay += HoursPerSecond * Time.deltaTime;
            TimeOfDay %= 24; //Modulus to ensure always between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateFogIntensity(float timePercent)
    {
        //Set ambient and fog
        if (timePercent >= 0.25 && timePercent <= 0.75)
        {
            float result = FogMin;
            if (timePercent <= 0.35f)
                result = Utils.MapRange(timePercent, 0.25f, 0.35f, FogMax, FogMin);
            if (timePercent >= 0.65f)
                result = Utils.MapRange(timePercent, 0.55f, 0.75f, FogMin, FogMax);
            RenderSettings.fogDensity = result;
        }
        else
            RenderSettings.fogDensity = FogMax;
    }

    private void UpdateLighting(float timePercent)
    {
        UpdateFogIntensity(timePercent);

        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }
        //Search scene for light that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }

    public float GetTimeOfDay() { return TimeOfDay; }
}