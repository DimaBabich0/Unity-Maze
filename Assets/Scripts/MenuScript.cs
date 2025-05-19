using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
struct KeyElement
{
    public string color;
    public Image image;
}

public class MenuScript : MonoBehaviour
{
    [SerializeField] private KeyElement[] inventoryImages;

    private GameObject content;
    private float startTimeScale;
    private bool isMuted;

    private Slider singleSfxSlider;
    private Slider reusableSfxSlider;
    private Slider musicSlider;
    private Toggle muteToggle;

    private float defaultSingleSfxVolume;
    private float defaultReusableSfxVolume;
    private float defaultMusicVolume;
    private bool defaultIsMuted;

    void Start()
    {
        SetDefaultSettings();
        content = transform.Find("Content").gameObject;

        singleSfxSlider = transform.Find("Content/Sounds/SliderSingleSFX").gameObject.GetComponent<Slider>();
        reusableSfxSlider = transform.Find("Content/Sounds/SliderReusableSFX").gameObject.GetComponent<Slider>();
        musicSlider = transform.Find("Content/Sounds/SliderMusic").gameObject.GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/ToggleMute").gameObject.GetComponent<Toggle>();

        LoadSaveSettings();

        OnMuteValueChanged(isMuted);

        startTimeScale = Time.timeScale;
        HideMenu();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (content.activeInHierarchy) HideMenu();
            else ShowMenu();
        }
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetFloat("singleSfxVolume", singleSfxSlider.value);
        PlayerPrefs.SetFloat("reusableSfxVolume", reusableSfxSlider.value);
        PlayerPrefs.SetFloat("musicVolume", musicSlider.value);
        PlayerPrefs.SetInt("isMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetDefaultSettings()
    {
        defaultSingleSfxVolume = GameState.singleSfxVolume;
        defaultReusableSfxVolume = GameState.reusableSfxVolume;
        defaultMusicVolume = GameState.musicVolume;
        defaultIsMuted = false;
    }
    private void LoadSaveSettings()
    {
        if (PlayerPrefs.HasKey("singleSfxVolume"))
            GameState.singleSfxVolume = singleSfxSlider.value = PlayerPrefs.GetFloat("singleSfxVolume");
        else
            singleSfxSlider.value = defaultSingleSfxVolume;

        if (PlayerPrefs.HasKey("reusableSfxVolume"))
            GameState.reusableSfxVolume = reusableSfxSlider.value = PlayerPrefs.GetFloat("reusableSfxVolume");
        else
            reusableSfxSlider.value = defaultReusableSfxVolume;

        if (PlayerPrefs.HasKey("musicVolume"))
            GameState.musicVolume = musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        else
            musicSlider.value = defaultMusicVolume;

        if (PlayerPrefs.HasKey("isMuted"))
            muteToggle.isOn = isMuted = PlayerPrefs.GetInt("isMuted") == 1 ? true : false;
        else
            isMuted = muteToggle.isOn = defaultIsMuted;
    }

    private void HideMenu()
    {
        content.SetActive(false);
        Time.timeScale = startTimeScale;
    }
    private void ShowMenu()
    {
        content.SetActive(true);
        startTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;

        for (int i = 0; i < inventoryImages.Length; i++)
        {
            if (GameState.inventory.ContainsKey($"key{inventoryImages[i].color}"))
            {
                inventoryImages[i].image.enabled = true;
            }
            else
            {
                inventoryImages[i].image.enabled = false;
            }
        }
    }

    public void OnSingleSfxValueChanged(System.Single volume)
    {
        if (!isMuted)
        {
            GameState.singleSfxVolume = volume;
        }
    }
    public void OnReusableSfxValueChanged(System.Single volume)
    {
        if (!isMuted)
        {
            GameState.reusableSfxVolume = volume;
        }
    }
    public void OnMusicValueChanged(System.Single volume)
    {
        if (!isMuted)
        {
            GameState.musicVolume = volume;
        }
    }
    public void OnMuteValueChanged(System.Boolean isMute)
    {
        isMuted = isMute;
        if (isMute)
        {
            GameState.singleSfxVolume = 0.0f;
            GameState.reusableSfxVolume = 0.0f;
            GameState.musicVolume = 0.0f;
        }
        else
        {
            GameState.singleSfxVolume = singleSfxSlider.value;
            GameState.reusableSfxVolume = reusableSfxSlider.value;
            GameState.musicVolume = musicSlider.value;
        }
    }

    public void OnContinueClick()
    {
        HideMenu();
    }
    public void OnDefaultsClick()
    {
        singleSfxSlider.value = defaultSingleSfxVolume;
        reusableSfxSlider.value = defaultReusableSfxVolume;
        musicSlider.value = defaultMusicVolume;
        muteToggle.isOn = defaultIsMuted;
    }
    public void OnExitClick()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        #if UNITY_STANDALONE
        Application.Quit();
        #endif
    }
}
