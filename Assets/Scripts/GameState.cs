using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameState
{
    public static Dictionary<string, int> inventory { get; } = new Dictionary<string, int>();

    #region bool isDay
    private static bool _isDay = true;
    public static bool isDay
    {
        get => _isDay;
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                Notify(nameof(isDay));
            }
        }
    }
    #endregion

    #region bool isFpv
    private static bool _isFpv = false;
    public static bool isFpv
    {
        get => _isFpv;
        set
        {
            if (_isFpv != value)
            {
                _isFpv = value;
                Notify(nameof(isFpv));
            }
        }
    }
    #endregion

    #region float singleSfxVolume
    private static float _singleSfxVolume = 0.1f;
    public static float singleSfxVolume
    {
        get => _singleSfxVolume;
        set
        {
            if (_singleSfxVolume != value)
            {
                _singleSfxVolume = value;
                Notify(nameof(singleSfxVolume));
            }
        }
    }
    #endregion

    #region float reusableSfxVolume
    private static float _reusableSfxVolume = 0.1f;
    public static float reusableSfxVolume
    {
        get => _reusableSfxVolume;
        set
        {
            if (_reusableSfxVolume != value)
            {
                _reusableSfxVolume = value;
                Notify(nameof(reusableSfxVolume));
            }
        }
    }
    #endregion

    #region float musicVolume
    private static float _musicVolume = 0.05f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(musicVolume));
            }
        }
    }
    #endregion

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();
    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
        listener(null);
    }
    public static void RemoveListener(Action<string> listener)
    {
        listeners.Remove(listener);
    }
    private static void Notify(string fieldName)
    {
        foreach (Action<string> listener in listeners)
        {
            listener.Invoke(fieldName);
        }
    }
    #endregion

    #region Set Property
    public static void SetProperty(string name, object value)
    {
        var prop = typeof(GameState).GetProperty(name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
        
        if (prop == null)
        {
            UnityEngine.Debug.LogError($"Error prop setting: property {name} not found (value: {value})");
        }
        prop.SetValue(null, value);
    }
    #endregion
}
