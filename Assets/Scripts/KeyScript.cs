using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    [SerializeField] public string keyColor = "Red";
    
    [SerializeField] private GameObject prevGate = null;
    private bool isPrevGateOpen = true;

    private GameObject content;
    private Image indicatorImage;
    [SerializeField] private float timeout = 5.0f;
    private float leftTime;
    private bool isInTime = true;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        indicatorImage = transform.Find("Indicator/Canvas/Foreground").GetComponent<Image>();
        indicatorImage.fillAmount = 1.0f;
        leftTime = timeout;

        isPrevGateOpen = prevGate == null;

        indicatorImage.fillAmount = leftTime / timeout;
        indicatorImage.color = new Color(
            Mathf.Clamp01(2 * (1 - indicatorImage.fillAmount)),
            Mathf.Clamp01(2 * indicatorImage.fillAmount),
            0.0f
        );

        isInTime = true;
    }

    void Update()
    {
        content.transform.Rotate(0, Time.deltaTime * 50f, 0);

        if (prevGate != null)
        {
            isPrevGateOpen = prevGate.GetComponentInChildren<GateScript>().isOpen;
        }

        if (leftTime >= 0 && isPrevGateOpen)
        {
            indicatorImage.fillAmount = leftTime / timeout;
            indicatorImage.color = new Color(
                Mathf.Clamp01(2 * (1 - indicatorImage.fillAmount)),
                Mathf.Clamp01(2 * indicatorImage.fillAmount),
                0.0f
            );

            leftTime -= Time.deltaTime;
            if (leftTime < 0)
            {
                isInTime = false;
            }
        }
    }

    private void OnTriggerEnter(Collider obj)
    {
        if (obj.name == "Player")
        {
            if (!GameState.inventory.ContainsKey($"key{keyColor}"))
                GameState.inventory.Add($"key{keyColor}", 1);

            GameEventSystem.TriggerEvent(new GameEvent
            {
                type = $"isKey{keyColor}Collected",
                payload = isInTime,
                toast = $"You find a {keyColor.ToLower()} key.\nYou can now open {keyColor.ToLower()} gates.",
                toastTimer = 3f,
                sound = isInTime ? EffectsSounds.keyPickUpInTime : EffectsSounds.keyPickUpOutOfTime,
            });
            Destroy(this.gameObject);
        }
    }
}
