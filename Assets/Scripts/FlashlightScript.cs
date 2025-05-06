using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    
    public static float charge = 1.0f;
    private float chargeLifetime = 60.0f;

    private float timer = 0f;
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

        if (GameState.isFpv && !GameState.isDay)
        {
            this.transform.position = player.transform.position;
            this.transform.forward = Camera.main.transform.forward;
            _light.intensity = charge;

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
            charge = Mathf.Clamp01(charge - cost);
            counterCost += cost;

            timer += Time.deltaTime;
            if (timer >= interval)
            {
                Debug.Log($"Angle: {_light.spotAngle:F2}; Charge: {charge:F2}; Consumption in second: {counterCost:F4}");
                timer = 0f;
                counterCost = 0f;
            }
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }
}
