using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemWithAudioManager : MonoBehaviour
{
    public AudioManager audioManager;
    public string audioClipName;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            {
            audioManager.Play("4ChiliEating", audioManager.runot);
            }
    }
}
