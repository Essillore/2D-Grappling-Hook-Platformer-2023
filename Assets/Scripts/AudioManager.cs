using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public Sound[] musics;

    public Sound[] runot;


    public static AudioManager instance;


    // Start is called before the first frame update
    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start()
    {

    }

    //PLaySounds-----------------------------------------------------------------------PlaySounds
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
        s.isPlaying = true; // Update the isPlaying flag
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Stop();
        s.isPlaying = false; // Update the isPlaying flag
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s != null && s.isPlaying;
    }

    public void SetSoundVolume(float volume)
    {
        foreach(Sound s in sounds)
        {
            s.source.volume = volume;
        }
    }

    //Play Music ------------------------------------------------------------------------Play Music

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + "not found!");
            return;
        }
        s.source.Play();
        s.isPlaying = true; // Update the isPlaying flag
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Music: " + name + "not found!");
            return;
        }
        s.source.Stop();
        s.isPlaying = false; // Update the isPlaying flag
    }

    public bool IsPlayingMusic(string name)
    {
        Sound s = Array.Find(musics, sound => sound.name == name);
        return s != null && s.isPlaying;
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Sound s in musics)
        {
            s.source.volume = volume;
        }
    }

    // Play Runot -------------------------------------------------------------------- Play runot
    public void PlayRuno(string name)
    {
        Sound s = Array.Find(runot, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Play();
        s.isPlaying = true; // Update the isPlaying flag
    }

    public void StopRuno(string name)
    {
        Sound s = Array.Find(runot, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return;
        }
        s.source.Stop();
        s.isPlaying = false; // Update the isPlaying flag
    }

    public bool IsPlayingRuno(string name)
    {
        Sound s = Array.Find(runot, sound => sound.name == name);
        return s != null && s.isPlaying;
    }

    public void SetRunoVolume(float volume)
    {
        foreach (Sound s in runot)
        {
            s.source.volume = volume;
        }
    }

    // Other sounds ------------------------------------------------------------------- Other sounds

    public void PlayRandomFootstep()
    {
        List<Sound> footstepSounds = new List<Sound>();

        // Iterate through your sounds array and add footstep sounds to the list
        foreach (Sound sound in sounds)
        {
            if (sound.name.StartsWith("Footstep"))
            {
                footstepSounds.Add(sound);
            }
        }

        // Check if there are any footstep sounds
        if (footstepSounds.Count > 0)
        {
            // Choose a random footstep sound from the list
            Sound randomFootstep = footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Count)];

            // Play the chosen random footstep sound
            randomFootstep.source.Play();
        }
    }
}