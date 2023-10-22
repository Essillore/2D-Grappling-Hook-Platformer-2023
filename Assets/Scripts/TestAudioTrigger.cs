using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAudioTrigger : MonoBehaviour
{
    public AudioManager audioManager;

    private void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioManager.SetRunoVolume(0.3f);
            print("osuit");
            audioManager.Play("4ChiliEating", audioManager.runot);
            print("audio soi");
        }
    }
}
