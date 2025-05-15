using System.Collections.Generic;
using UnityEngine;

public class EffectsScript : MonoBehaviour
{
    private AudioSource keyPickUpInTimeSource;
    private AudioSource keyPickUpOutOfTimeSource;
    private AudioSource batteryPickUpSource;

    void Start()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        keyPickUpInTimeSource = audioSources[0];
        keyPickUpOutOfTimeSource = audioSources[1];
        batteryPickUpSource = audioSources[2];

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(onGameStateChanged);
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (!string.IsNullOrEmpty(gameEvent.sound))
        {
            switch (gameEvent.sound)
            {
                case EffectsSounds.keyPickUpInTime: keyPickUpInTimeSource.Play(); break;
                case EffectsSounds.keyPickUpOutOfTime: keyPickUpOutOfTimeSource.Play(); break;
                case EffectsSounds.batteryPickUp: batteryPickUpSource.Play(); break;
                default: Debug.LogWarning($"Sound not found in effectPlays: {gameEvent.sound}"); break;
            }
        }
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.singleSfxVolume))
        {
            keyPickUpInTimeSource.volume =
            keyPickUpOutOfTimeSource.volume =
            batteryPickUpSource.volume = GameState.singleSfxVolume;
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(onGameStateChanged);
    }
}
