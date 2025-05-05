using System.Linq;
using UnityEditor.SettingsManagement;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    private Light[] dayLights;
    private Light[] nightLights;
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxNight;
    private bool isDay;

    void Start()
    {
        dayLights = GameObject
            .FindGameObjectsWithTag("Day")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        nightLights = GameObject
            .FindGameObjectsWithTag("Night")
            .Select(g => g.GetComponent<Light>())
            .ToArray();

        isDay = true;
        ChangeToDay();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            isDay = !isDay;
            if (isDay)
            {
                ChangeToDay();
            }
            else
            {
                ChangeToNight();
            }
        }
    }

    private void ChangeToDay()
    {
        foreach (Light light in dayLights)
            light.intensity = 1.0f;
        foreach (Light light in nightLights)
            light.intensity = 0.0f;

        RenderSettings.ambientIntensity = 1.0f;
        RenderSettings.reflectionIntensity = 1.0f;
        RenderSettings.skybox = skyboxDay;
    }

    private void ChangeToNight()
    {
        foreach (Light light in dayLights)
            light.intensity = 0.0f;
        foreach (Light light in nightLights)
            light.intensity = 1.0f;

        RenderSettings.ambientIntensity = 0.2f;
        RenderSettings.reflectionIntensity = 0.2f;
        RenderSettings.skybox = skyboxNight;
    }
}
