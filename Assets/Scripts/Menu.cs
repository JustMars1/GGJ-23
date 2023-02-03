using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public Slider musicSlider, sfxSlider;
    public Button playBt, optionsBt, closeBt;
    public RectTransform startPanel, optionsPanel;

    const string MUSIC_VOLUME_KEY = "MusicVolume";
    const string SFX_VOLUME_KEY = "SFXVolume";

    static float musicVolume = 0.5f;
    static float sfxVolume = 0.5f;

    void Awake()
    {
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        sfxSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChanged(); });

        playBt.onClick.AddListener(Play);
        optionsBt.onClick.AddListener(Options);
        closeBt.onClick.AddListener(Close);
    }

    void OnMusicVolumeChanged()
    {
        musicVolume = musicSlider.value;
        AudioPlayer.mixer.SetFloat(MUSIC_VOLUME_KEY, Mathf.Log10(musicVolume) * 20);
    }

    void OnSFXVolumeChanged()
    {
        sfxVolume = sfxSlider.value;
        AudioPlayer.mixer.SetFloat(SFX_VOLUME_KEY, Mathf.Log10(musicVolume) * 20);
    }

    void Play()
    {

    }

    void Options()
    {
        optionsPanel.gameObject.SetActive(true);
        closeBt.gameObject.SetActive(true);
        startPanel.gameObject.SetActive(false);
    }

    void Close()
    {
        optionsPanel.gameObject.SetActive(false);
        closeBt.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(true);
    }
}
