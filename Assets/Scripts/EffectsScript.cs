using System.Collections.Generic;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyPickUpInTimeSound;
    private AudioSource keyPickUpOutOfTimeSound;
    private AudioSource batteryPickUpSound;


    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        keyPickUpInTimeSound = audioSources[0];
        keyPickUpOutOfTimeSound = audioSources[1];
        batteryPickUpSound = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (!string.IsNullOrEmpty(gameEvent.sound))
        {
            switch (gameEvent.sound)
            {
                case EffectsSounds.keyPickUpInTime: keyPickUpInTimeSound.Play(); break;
                case EffectsSounds.keyPickUpOutOfTime: keyPickUpOutOfTimeSound.Play(); break;
                case EffectsSounds.batteryPickUp: batteryPickUpSound.Play(); break;
                default: Debug.LogWarning($"Sound not found in effectPlays: {gameEvent.sound}"); break;
            }
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
    }
}
