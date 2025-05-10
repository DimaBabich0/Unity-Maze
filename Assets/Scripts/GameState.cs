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

    #region bool isKeyRedInTime
    private static bool _isKeyRedInTime = true;
    public static bool isKeyRedInTime
    {
        get => _isKeyRedInTime;
        set
        {
            if (_isKeyRedInTime != value)
            {
                _isKeyRedInTime = value;
                Notify(nameof(isKeyRedInTime));
            }
        }
    }
    #endregion

    #region bool isKeyRedCollected
    private static bool _isKeyRedCollected = false;
    public static bool isKeyRedCollected
    {
        get => _isKeyRedCollected;
        set
        {
            if (_isKeyRedCollected != value)
            {
                _isKeyRedCollected = value;
                Notify(nameof(isKeyRedCollected));
            }
        }
    }
    #endregion

    #region bool isKeyBlueInTime
    private static bool _isKeyBlueInTime = true;
    public static bool isKeyBlueInTime
    {
        get => _isKeyBlueInTime;
        set
        {
            if (_isKeyBlueInTime != value)
            {
                _isKeyBlueInTime = value;
                Notify(nameof(isKeyBlueInTime));
            }
        }
    }
    #endregion

    #region bool isKeyBlueCollected
    private static bool _isKeyBlueCollected = false;
    public static bool isKeyBlueCollected
    {
        get => _isKeyBlueCollected;
        set
        {
            if (_isKeyBlueCollected != value)
            {
                _isKeyBlueCollected = value;
                Notify(nameof(isKeyBlueCollected));
            }
        }
    }
    #endregion

    #region bool isKeyGreenInTime
    private static bool _isKeyGreenInTime = true;
    public static bool isKeyGreenInTime
    {
        get => _isKeyGreenInTime;
        set
        {
            if (_isKeyGreenInTime != value)
            {
                _isKeyGreenInTime = value;
                Notify(nameof(isKeyGreenInTime));
            }
        }
    }
    #endregion

    #region bool isKeyGreenCollected
    private static bool _isKeyGreenCollected = false;
    public static bool isKeyGreenCollected
    {
        get => _isKeyGreenCollected;
        set
        {
            if (_isKeyGreenCollected != value)
            {
                _isKeyGreenCollected = value;
                Notify(nameof(isKeyGreenCollected));
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
