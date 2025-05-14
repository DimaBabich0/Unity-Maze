using UnityEngine;

public class MusicScript : MonoBehaviour
{
    private AudioSource musicSource;

    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }
}
