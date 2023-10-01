using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTemperature : MonoBehaviour
{
    public PlayerControllerScript playerController;
    public bool isMoving = false;

    [Header("Temperature Rising")]
    public float currentPlayerTemperature;
    public float initialPlayerTemperature;
    public float temperatureRiseRate = 0.02f;
    public float someScalingFactor = 0.04f;

    [Header("Temperature Falling")]
    public float coolingRate;
    public float howQuicklyDrops = 20.0f;
    public float clampForTemperatureDrop = 4f;
    public float environmentTemperature = 4f;

    [Header("Temperature UI")]
    public TMP_Text playerTemperatureText;
    public TMP_Text environmentTemperatureText;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerTemperature = initialPlayerTemperature;
    }

    // Update is called once per frame
    void Update()
    {
        //Temperature change of player

        //player temperature will fall, more if the difference between player and environment is bigger.

        // Calculate the temperature difference between the player and environment
        float temperatureDifference = currentPlayerTemperature - environmentTemperature;

        // Define a rate of cooling based on the temperature difference and clamped to a certain range
        coolingRate = Mathf.Clamp(temperatureDifference / howQuicklyDrops, -clampForTemperatureDrop, clampForTemperatureDrop);

        // Adjust the player's temperature based on the cooling rate
        currentPlayerTemperature -= coolingRate * Time.deltaTime;
        if (isMoving == true) //warming
        {
            //player temperature will rise, proportionally more if 
            //difference between current temperature and normal temperature is big.
            float initialAndCurrentDifference = initialPlayerTemperature - currentPlayerTemperature;

            // temperatureRiseRate = Mathf.Clamp(initialAndCurrentDifference / 10.0f, -1.0f, 1.0f);
            // Calculate a temperature rise rate that is proportional to the temperature difference
            float proportionalRiseRate = initialAndCurrentDifference * someScalingFactor;

            currentPlayerTemperature += (temperatureRiseRate + proportionalRiseRate) * Time.deltaTime;

        }
        else if (isMoving == false) //cooling
        {

         
        }
            playerTemperatureText.text = currentPlayerTemperature.ToString("F1") + " °C"; // Display the temperature with one decimal place
            environmentTemperatureText.text = environmentTemperature + " °C";
    }
}
