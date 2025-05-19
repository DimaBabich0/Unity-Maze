using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private static MusicScript prevInstance;
    private AudioSource musicSource;

    void Start()
    {
        if (prevInstance != null)
        {
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            prevInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }

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
