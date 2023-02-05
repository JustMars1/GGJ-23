using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public AudioClip buttonSoundClip;
    public Slider musicSlider, sfxSlider;
    public Button playBt, optionsBt, quitBt, closeBt;
    public RectTransform startPanel, optionsPanel, pausePanel;
    public Toggle fullScreenToggle, vSyncToggle;

    const string MUSIC_VOLUME_KEY = "MusicVolume";
    const string SFX_VOLUME_KEY = "SFXVolume";

    const float VOLUME_MIN_VALUE = 0.00001f;

    bool initialized = false;

    void Awake()
    {
        // Volume slider values range from 0 to 10 (integers)
        sfxSlider.value = PlayerPrefs.GetInt(SFX_VOLUME_KEY, 2);
        musicSlider.value = PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 2);
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });

        playBt.onClick.AddListener(Play);
        optionsBt.onClick.AddListener(Options);
        closeBt.onClick.AddListener(Close);
        quitBt.onClick.AddListener(Quit);

        fullScreenToggle.isOn = QualitySettings.vSyncCount == 1;
        vSyncToggle.isOn = Screen.fullScreen;

        fullScreenToggle.onValueChanged.AddListener(delegate { OnFullScreenToggled(); });
        vSyncToggle.onValueChanged.AddListener(delegate { OnVSyncToggled(); });
    }

    void Start()
    {
        OnSFXVolumeChanged();
        OnMusicVolumeChanged();
        OnFullScreenToggled();
        OnVSyncToggled();

        initialized = true;
    }

    public void PlayBtClickSound()
    {
        if (initialized)
        {
            AudioPlayer.Play(buttonSoundClip, isMusic: false, variablePitch: true, variableVolume: true);
        }
    }

    void OnSFXVolumeChanged()
    {
        PlayerPrefs.SetInt(SFX_VOLUME_KEY, (int)sfxSlider.value);
        float sfxVolume = Mathf.Clamp((float)sfxSlider.value / sfxSlider.maxValue, VOLUME_MIN_VALUE, 1.0f);
        AudioPlayer.mixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(sfxVolume) * 20);
    }

    void OnMusicVolumeChanged()
    {
        PlayerPrefs.SetInt(MUSIC_VOLUME_KEY, (int)musicSlider.value);
        float musicVolume = Mathf.Clamp((float)musicSlider.value / musicSlider.maxValue, VOLUME_MIN_VALUE, 1.0f);
        AudioPlayer.mixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(musicVolume) * 20);
    }

    void Play()
    {
        PlayBtClickSound();
        GameManager.Instance.LoadNextLevel();
    }

    void Options()
    {
        optionsPanel.gameObject.SetActive(true);
        sfxSlider.Select();

        PlayBtClickSound();
    }

    void Close()
    {
        optionsPanel.gameObject.SetActive(false);
        optionsBt.Select();
        PlayBtClickSound();
    }

    void Quit() 
    {
        PlayBtClickSound();
        Application.Quit();
    }

    void OnFullScreenToggled()
    {
        if (fullScreenToggle.isOn)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
        else
        {
            Screen.SetResolution(1600, 900, FullScreenMode.Windowed);
        }

        PlayBtClickSound();
    }

    void OnVSyncToggled()
    {
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
        PlayBtClickSound();
    }
}
