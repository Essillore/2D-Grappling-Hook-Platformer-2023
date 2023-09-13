using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureTimer : MonoBehaviour
{
    public float timer;
    public GameObject player;
    public float playerTemperature = 38f;
    public TMP_Text playerTemperatureText;
    public float environmentTemperature = 20f;
    public TMP_Text environmentTemperatureText;
    // Start is called before the first frame update
    void Start()
    {
        environmentTemperatureText.text = environmentTemperature + " °C";
        StartCoroutine(TemperatureDropTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            DropPlayerTemperature();
            StartCoroutine(TemperatureDropTimer());
        }
    }

    public IEnumerator TemperatureDropTimer()
    {
     timer = 3f;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
        }
    }

    public void DropPlayerTemperature()
    {
        playerTemperature -= 1f;
        playerTemperatureText.text = playerTemperature + " °C";
    }

}
