using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioManager audioManager;

    public bool sounds;
    public bool musics;
    public bool runot;

    public void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void OnVolumeSliderValueChanged(float volume)
    {
        if (sounds) 
        {
            audioManager.SetSoundVolume(volume); 
        }
        if(musics) 
        {
            audioManager.SetMusicVolume(volume);
        }
        if(runot) 
        {
            audioManager.SetRunoVolume(volume);
        }
    }
}
