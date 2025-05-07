using System.Linq;
using UnityEditor.SettingsManagement;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;

    void Start()
    {
        GameState.AddListener(onGameStateChanged);

        dayLights = GameObject
            .FindGameObjectsWithTag("Day")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        nightLights = GameObject
            .FindGameObjectsWithTag("Night")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        ToggleLight();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            GameState.isDay = !GameState.isDay;
    }  

    private void ToggleLight()
    {
        //Debug.Log($"LightScript: isDay = {GameState.isDay}; isFpv = {GameState.isFpv}");
        if (GameState.isDay) // day lights
        {
            foreach (Light light in dayLights)
                light.intensity = 1.0f;

            foreach (Light light in nightLights)
                light.intensity = 0.0f;

            RenderSettings.ambientIntensity = 1.0f;
            RenderSettings.reflectionIntensity = 1.0f;
        }
        else // night lights
        {
            foreach (Light light in dayLights)
                light.intensity = 0.0f;

            foreach (Light light in nightLights)
                light.intensity = GameState.isFpv ? 0.0f : 1.0f;

            RenderSettings.ambientIntensity = 0.2f;
            RenderSettings.reflectionIntensity = 0.2f;
        }
    }

    private void FpvChanged()
    {
        Debug.Log($"LightScript: isDay = {GameState.isDay}; isFpv = {GameState.isFpv}");
        if (!GameState.isDay)
        {
            foreach (Light light in nightLights)
                light.intensity = GameState.isFpv ? 0.0f : 1.0f;
        }
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
            ToggleLight();
        else if (fieldName == nameof(GameState.isFpv))
            FpvChanged();
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
