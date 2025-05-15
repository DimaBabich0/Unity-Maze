using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxNight;

    void Start()
    {
        GameState.AddListener(onGameStateChanged);
        ChangeSkybox();
    }

    private void ChangeSkybox()
    {
        RenderSettings.skybox = GameState.isDay ? skyboxDay : skyboxNight;
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.isDay))
            ChangeSkybox();
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
