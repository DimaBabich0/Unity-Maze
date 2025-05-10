using UnityEngine;

public class GateScript : MonoBehaviour
{
    [SerializeField] private Vector3 openDirection = Vector3.forward;
    [SerializeField] private float size = -0.6f;

    private const float openTimeFast = 2.0f;
    private const float openTimeSlow = 10.0f;
    private float openTime = 0;

    [SerializeField] private string keyColor = "Red";
    private bool isKeyCollected = false;
    private bool isKeyInTime = true;
    public bool isOpen = false;

    void Start()
    {
        GameState.AddListener(onGameStateChanged);
    }

    void Update()
    {
        if (isOpen && -transform.localPosition.magnitude > size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isKeyCollected)
            {
                ToasterScript.Toast($"The gate is closed.\nYou need to find {keyColor.ToLower()} key to open it.", 4f);
            }
            else if (!isOpen)
            {
                ToasterScript.Toast($"You open the {keyColor.ToLower()} gate.", 1f);
                isOpen = true;
                openTime = isKeyInTime ? openTimeFast : openTimeSlow;
            }
        }
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == $"isKey{keyColor}Collected")
        {
            isKeyCollected = true;
        }
        else if (fieldName == $"isKey{keyColor}InTime")
        {
            isKeyInTime = false;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
