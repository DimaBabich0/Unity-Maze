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
    public bool isOpen { get; private set; } = false;
    private bool isOpened = false;

    private AudioSource[] audioSources;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        if (isOpen && !isOpened && -transform.localPosition.magnitude > size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection);
            if (-transform.localPosition.magnitude <= size)
            {
                isOpened = true;
                if (audioSources != null)
                {
                    foreach (var source in audioSources)
                    {
                        source.Stop();
                    }
                }
            }
        }

        if (audioSources == null || audioSources.Length == 0) return;
        else if (audioSources.Length == 1 && audioSources[0].isPlaying)
            audioSources[0].volume = Time.timeScale == 0.0f ? 0.0f : GameState.reusableSfxVolume;
        else if (audioSources.Length >= 2 && audioSources[0].isPlaying || audioSources[1].isPlaying)
        {
            audioSources[0].volume = audioSources[1].volume =
                Time.timeScale == 0.0f ? 0.0f : GameState.reusableSfxVolume;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isKeyCollected)
            {
                GameEventSystem.TriggerEvent(new GameEvent
                {
                    toast = $"The gate is closed.\nYou need to find {keyColor.ToLower()} key to open it.",
                    toastTimer = 4f,
                });
            }
            else if (!isOpen)
            {
                GameEventSystem.TriggerEvent(new GameEvent
                {
                    toast = $"You open the {keyColor.ToLower()} gate.",
                    toastTimer = 1f,
                });
                isOpen = true;
                openTime = isKeyInTime ? openTimeFast : openTimeSlow;

                if (audioSources == null || audioSources.Length == 0) return;
                if (audioSources.Length == 1)
                    audioSources[0].Play();
                else if (audioSources.Length >= 2)
                {
                    (isKeyInTime ? audioSources[0] : audioSources[1]).Play();
                }
            }
        }
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.type == $"isKey{keyColor}Collected")
        {
            isKeyCollected = true;

            if (gameEvent.payload is bool payload)
                isKeyInTime = payload;
            else
            {
                Debug.LogWarning($"gameEvent: {gameEvent} had wrong type of payload (waiting for bool, but get {gameEvent.GetType()})");
                isKeyInTime = false;
            }
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
