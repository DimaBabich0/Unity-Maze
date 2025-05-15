using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource musicSource;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
        GameState.AddListener(onGameStateChanged);
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.musicVolume))
        {
            musicSource.volume = GameState.musicVolume;
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(onGameStateChanged);
    }
}
