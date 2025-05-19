using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class SoundNameAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(SoundNameAttribute))]
public class SoundNameDrawer : PropertyDrawer
{
    private static string[] allSoundNames;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // load every effect sound once
        if (allSoundNames == null)
        {
            var fields = typeof(EffectsSounds).GetFields(BindingFlags.Public | BindingFlags.Static);
            allSoundNames = Array.ConvertAll(fields, f => f.GetValue(null).ToString());
        }

        // get list of selected items from inspector
        HashSet<string> selectedNames = new HashSet<string>();
        var listProperty = property
            .serializedObject.FindProperty(property.propertyPath.Replace(".name", ""))
            .serializedObject.FindProperty("sounds");

        for (int i = 0; i < listProperty.arraySize; i++)
        {
            var element = listProperty.GetArrayElementAtIndex(i);
            var nameProp = element.FindPropertyRelative("name");

            if (nameProp != property && !string.IsNullOrEmpty(nameProp.stringValue))
                selectedNames.Add(nameProp.stringValue);
        }

        // the current value should also be in the list (otherwise dropdown "drop")
        if (!selectedNames.Contains(property.stringValue))
            selectedNames.Add(property.stringValue);

        // leave only available variables
        var availableNames = Array.FindAll(allSoundNames, name => !selectedNames.Contains(name) || name == property.stringValue);

        // selected index
        int index = Array.IndexOf(availableNames, property.stringValue);
        if (index < 0) index = 0;

        EditorGUI.BeginProperty(position, label, property);
        int newIndex = EditorGUI.Popup(position, label.text, index, availableNames);
        property.stringValue = availableNames.Length > 0 ? availableNames[newIndex] : "";
        EditorGUI.EndProperty();
    }
}

[System.Serializable]
public class SoundEntry
{
    [SoundName] public string name;
    public AudioClip clip;
}

public class EffectsManagerScript : MonoBehaviour
{
    private static EffectsManagerScript prevInstance;
    [SerializeField] private List<SoundEntry> sounds = new();
    private Dictionary<string, AudioSource> audioSourcesByName = new();

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

        foreach (var entry in sounds)
        {
            if (string.IsNullOrEmpty(entry.name) || entry.clip == null) continue;

            var source = gameObject.AddComponent<AudioSource>();
            source.clip = entry.clip;
            source.playOnAwake = false;
            source.volume = GameState.singleSfxVolume;

            audioSourcesByName[entry.name] = source;
        }

        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(onGameStateChanged);
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (!string.IsNullOrEmpty(gameEvent.sound))
        {
            if (audioSourcesByName.TryGetValue(gameEvent.sound, out AudioSource source))
                source.Play();
            else
                Debug.LogWarning($"Sound not found: {gameEvent.sound}");
        }
    }

    private void onGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.singleSfxVolume))
        {
            foreach (AudioSource source in audioSourcesByName.Values)
            {
                source.volume = GameState.singleSfxVolume;
            }
        }
    }

    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(onGameStateChanged);
    }
}
