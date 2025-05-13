using Unity.VisualScripting;
using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    
    public static float charge = 1.0f;
    private float chargeLifetime = 60.0f;

    private float counterTimer = 0f;
    private float counterCost = 0f;
    private float interval = 1f;
    private const float minAngle = 15f;
    private const float maxAngle = 45f;

    void Start()
    {
        _light = GetComponent<Light>();

        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.Log("FlashlightScript: Player not found");
        }
    }

    void Update()
    {
        if (player == null) return;

        this.transform.position = player.transform.position;
        this.transform.forward = Camera.main.transform.forward;

        if (GameState.isFpv && !GameState.isDay)
        {
            _light.intensity = Mathf.Clamp01(charge);

            if (Input.GetKey(KeyCode.Q) && _light.spotAngle >= minAngle)
            {
                _light.spotAngle -= 10f * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.E) && _light.spotAngle <= maxAngle)
            {
                _light.spotAngle += 10f * Time.deltaTime;
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("Charge reset");
                charge = 1.0f;
            }

            float cost = Time.deltaTime / chargeLifetime * (_light.spotAngle / 30);
            charge = charge < 0 ? 0 : charge - cost;
            counterCost += cost;

            counterTimer += Time.deltaTime;
            if (counterTimer >= interval)
            {
                Debug.Log($"Angle: {_light.spotAngle:F2}; Charge: {charge:F2}; Consumption in second: {counterCost:F4}");
                counterTimer = 0f;
                counterCost = 0f;
            }
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag("Battery"))
        {
            float batteryCharge = obj.GetComponent<BatteryScript>().batteryCharge;
            charge += batteryCharge;
            GameEventSystem.TriggerEvent(new GameEvent
            {
                type = "battery",
                toast = $"You find a battery with {batteryCharge.ToString("F2").Replace(',', '.')} charge.\n" +
                $"Now your charge is {charge.ToString("F2").Replace(',', '.')}",
                toastTimer = 5.0f,
                sound = EffectsSounds.batteryPickUp
            });
            //Debug.Log($"Battery collected; Charge: {charge:F2}");
            GameObject.Destroy(obj.gameObject);
        }
    }
}
