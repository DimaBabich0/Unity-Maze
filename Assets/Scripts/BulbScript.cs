using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BulbScript : MonoBehaviour
{
    private Light light;
    private Renderer bulb;

    [SerializeField] private Material bulbOff;
    [SerializeField] private Material bulbOn;

    void Start()
    {
        light = GetComponentInChildren<Light>();
        bulb = GetComponentsInChildren<Renderer>()
        .FirstOrDefault(r => r.gameObject != this.gameObject);

        if (bulbOff == null)
        {
            bulbOff = bulb.material;
        }
    }

    void Update()
    {
        if (light == null || bulb == null) return;
        bulb.material = light.intensity > 0 ? bulbOn : bulbOff;
    }
}
