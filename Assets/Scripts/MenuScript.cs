using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject content;

    //private GameObject sliderEffects;
    //private GameObject sliderMusic;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        HideMenu();

        //sliderEffects = transform.Find("SliderEffects").gameObject;
        //sliderMusic = transform.Find("SliderMusic").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (content.activeInHierarchy) HideMenu();
            else ShowMenu();
        }
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

    public void OnEffectsValueChanged(float volume)
    {
        GameState.effectsVolume = volume;
    }
    public void OnMusicValueChanged(float volume)
    {
        GameState.musicVolume = volume;
    }
    public void OnMuteValueChanged(bool isMute)
    {
        if (!isMute)
        {
            //GameState.effectsVolume = sliderEffects.GetComponent<Slider>().value;
            //GameState.musicVolume = sliderMusic.GetComponent<Slider>().value;
        }
        else
        {
            GameState.effectsVolume = 0.0f;
            GameState.musicVolume = 0.0f;
        }
    }
}
