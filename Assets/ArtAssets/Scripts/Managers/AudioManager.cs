using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour {
    //ı

    public static AudioManager Instance;
    public Sound[] sounds;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitializeSounds();
    }
    private void OnEnable()
    {
        EventsManager.onGameFinished += PlayMusicIfWin;
        EventsManager.onInitializeGame += StopMusics;
    }

    private void OnDisable()
    {
        EventsManager.onGameFinished -= PlayMusicIfWin;
        EventsManager.onInitializeGame -= StopMusics;

    }

    private void StopMusics(bool obj)
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            StopSFX(sounds[i].name);
        }
    }

    private void PlayMusicIfWin(bool isWin)
    {
        if(isWin)
        {
            PlaySFX("Win");
        } else
        {
            PlaySFX("Lose");
        }
    }

    private void InitializeSounds()
    {
        for(int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop; 
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        s.source.Play();

    }
    public void PlaySFX(string name, AudioClip clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }
        s.source.clip = clip;
        s.source.Play();

    }
    public void StopSFX(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null)
        {
            return;
        }

        s.source.Stop();
    }
}
