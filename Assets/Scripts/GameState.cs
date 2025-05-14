using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GameState
{
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

    #region float effectsVolume
    private static float _effectsVolume = 0.1f;
    public static float effectsVolume
    {
        get => _effectsVolume;
        set
        {
            if (_effectsVolume != value)
            {
                _effectsVolume = value;
                Notify(nameof(_effectsVolume));
            }
        }
    }
    #endregion

    #region float musicVolume
    private static float _musicVolume = 0.1f;
    public static float musicVolume
    {
        get => _musicVolume;
        set
        {
            if (_musicVolume != value)
            {
                _musicVolume = value;
                Notify(nameof(_musicVolume));
            }
        }
    }
    #endregion

    #region Change Notifier
    private static List<Action<string>> listeners = new List<Action<string>>();
    public static void AddListener(Action<string> listener)
    {
        listeners.Add(listener);
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
