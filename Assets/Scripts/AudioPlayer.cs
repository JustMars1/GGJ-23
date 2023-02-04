using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPlayer : MonoBehaviour
{
    public static AudioMixer mixer = Resources.Load<AudioMixer>("Mixer");

    static int musicCounter = 0, sfxCounter = 0;

    public static AudioSource Play(AudioClip clip, bool isMusic = false, bool variablePitch = false, bool variableVolume = false)
    {
        string goName = isMusic ? "Music" + musicCounter++ : "SFX" + sfxCounter++;
        AudioSource audio = new GameObject(goName).AddComponent<AudioSource>();

        audio.outputAudioMixerGroup = mixer.FindMatchingGroups(isMusic ? "Music" : "SFX")[0];
        audio.clip = clip;
        audio.volume = variableVolume ? Random.Range(0.9f, 1.0f) : 1.0f;
        audio.pitch = variablePitch ? Random.Range(0.95f, 1.05f) : 1.0f;
        audio.loop = isMusic;
        audio.Play();

        if (!isMusic)
        {
            Destroy(audio.gameObject, clip.length);
        }

        return audio;
    }
}
