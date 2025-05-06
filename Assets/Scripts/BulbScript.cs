using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BulbScript : MonoBehaviour
{
    private Light _light;
    private Renderer bulb;

    [SerializeField] private Material bulbOff;
    [SerializeField] private Material bulbOn;

    void Start()
    {
        _light = GetComponentInChildren<Light>();
        bulb = GetComponentsInChildren<Renderer>()
        .FirstOrDefault(r => r.gameObject != this.gameObject);

        if (bulbOff == null)
        {
            bulbOff = bulb.material;
        }

        GameState.AddListener(onGameStateChanged);
    }

    private void CheckMaterial()
    {
        if (!GameState.isDay && !GameState.isFpv)
            bulb.material = bulbOn;
        else
            bulb.material = bulbOff;
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
            CheckMaterial();
        else if (fieldName == nameof(GameState.isFpv))
            CheckMaterial();
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
