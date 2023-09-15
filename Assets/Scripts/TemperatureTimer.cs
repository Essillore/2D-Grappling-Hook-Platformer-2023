using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TemperatureTimer : MonoBehaviour
{
    public float timer;
    public float elapsedTime = 0f;
    public GameObject player;
    public float playerTemperature = 38f;
    public TMP_Text playerTemperatureText;
    public float environmentTemperature = 12f;
    public TMP_Text environmentTemperatureText;
    public float heatTransferCoefficient = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        environmentTemperatureText.text = environmentTemperature + " °C";
        StartCoroutine(TemperatureDropTimer());
        // StartCoroutine(NewtonianTemperatureDrop());
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0f)
        {
            DropPlayerTemperature();
            StartCoroutine(TemperatureDropTimer());
           // StartCoroutine(NewtonianTemperatureDrop());
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


        //    playerTemperature = -heatTransferCoefficient * (playerTemperature - environmentTemperature);
         //  playerTemperature = environmentTemperature + (38f - environmentTemperature) * Mathf.Log(-0.2f); Mathf.Exp(-0.2f);


    //ChatGPT-assisted-created-BROKEN-WIP TemperatureDrop that's proportional to the difference in environment and player temperature.
    //DOES NOT WORK AS INTENDED
    // Works, kinda, but too fast for game, and is messed up by the "moving, so temperature does not drop"-mechanic
    public IEnumerator NewtonianTemperatureDrop()
    {
        float initialPlayerTemperature = playerTemperature; // Store the initial temperature

        float temperatureChangeRate = 0.02f; // Rate of temperature change (1°C in 5 seconds)

        while (elapsedTime < 3f) // Adjust the time duration as needed
        {
            yield return new WaitForSeconds(1f);
            elapsedTime += 1f;

            // Calculate the new player temperature using Newton's Law of Cooling
            playerTemperature = environmentTemperature + (initialPlayerTemperature - environmentTemperature) * Mathf.Exp(-temperatureChangeRate * elapsedTime);

            playerTemperatureText.text = playerTemperature.ToString("F1") + " °C"; // Display the temperature with one decimal place
        }
    }

}
