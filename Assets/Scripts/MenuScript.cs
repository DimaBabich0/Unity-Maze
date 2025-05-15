using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject content;
    private bool isMuted;

    private Slider singleSfxSlider;
    private Slider reusableSfxSlider;
    private Slider musicSlider;
    private Toggle muteToggle;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        HideMenu();

        singleSfxSlider = transform.Find("Content/Sounds/SliderSingleSFX").gameObject.GetComponent<Slider>();
        reusableSfxSlider = transform.Find("Content/Sounds/SliderReusableSFX").gameObject.GetComponent<Slider>();
        musicSlider = transform.Find("Content/Sounds/SliderMusic").gameObject.GetComponent<Slider>();
        muteToggle = transform.Find("Content/Sounds/ToggleMute").gameObject.GetComponent<Toggle>();

        if (PlayerPrefs.HasKey("singleSfxVolume"))
            GameState.singleSfxVolume = singleSfxSlider.value = PlayerPrefs.GetFloat("singleSfxVolume");
        else
            singleSfxSlider.value = GameState.singleSfxVolume;

        if (PlayerPrefs.HasKey("reusableSfxVolume"))
            GameState.reusableSfxVolume = reusableSfxSlider.value = PlayerPrefs.GetFloat("reusableSfxVolume");
        else
            reusableSfxSlider.value = GameState.reusableSfxVolume;

        if (PlayerPrefs.HasKey("musicVolume"))
            GameState.musicVolume = musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        else
            musicSlider.value = GameState.musicVolume;

        if (PlayerPrefs.HasKey("isMuted"))
            muteToggle.isOn = isMuted = PlayerPrefs.GetInt("isMuted") == 1 ? true : false;
        else
            isMuted = muteToggle.isOn = false;

        OnMuteValueChanged(isMuted);
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

    private void HideMenu()
    {
        content.SetActive(false);
        Time.timeScale = 1.0f;
    }
    private void ShowMenu()
    {
        content.SetActive(true);
        Time.timeScale = 0.0f;
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
}
