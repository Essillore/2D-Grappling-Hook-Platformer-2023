using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerTemperature : MonoBehaviour
{
    public PlayerControllerScript playerController;
    public bool isMoving = false;

    [Header("Gamemode Temperature Death")]
    public bool temperatureKills;
    public PostProcessingController postProcessingController;

    [Header("Temperature Rising")]
    public static float currentPlayerTemperature;
    public float initialPlayerTemperature;
    public float temperatureRiseRate = 0.02f;
    public float someScalingFactor = 0.02f;

    [Header("Temperature Falling")]
    public float coolingRate;
    public float howQuicklyDrops = 10.0f;
    public float clampForTemperatureDrop = 2f;
    public float environmentTemperature = -2f;

    [Header("Temperature UI")]
    public TMP_Text playerTemperatureText;
    public TMP_Text environmentTemperatureText;

    // Start is called before the first frame update
    void Start()
    {
        postProcessingController = GameObject.Find("GlobalVolume").GetComponent<PostProcessingController>();
        FindTemperatureUI();
        currentPlayerTemperature = initialPlayerTemperature;
    }

    public void FindTemperatureUI()
    {
        playerTemperatureText = GameObject.Find("CharacterTempUI").GetComponent<TMP_Text>();
        environmentTemperatureText = GameObject.Find("EnvironmentTempUI").GetComponent<TMP_Text>();

    }

    // Update is called once per frame
    void Update()
    {
        //connect boolean of GameModeManager to player temperature, in a way that doesn't require PlayerTemperature to 
        //exist in options screen.
        temperatureKills = !GameModeManager.atmosphericTemperature;
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

        //Death on temperature
        if (currentPlayerTemperature <= 0 && temperatureKills == true)
        {
            playerController.Death();
        }

        //PostProcessing effects for level 1
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (currentPlayerTemperature > 30f)
            {
                //postProcessingController.LuminosityVsSatCurve();
            }
            else if (currentPlayerTemperature <= 30f && currentPlayerTemperature >= 20f)
            {
                postProcessingController.MasterCurveAdjustment();
            }
            else if (currentPlayerTemperature <= 20f && currentPlayerTemperature >= 10f)
            {
                postProcessingController.BlueCurveAdjustment();
            }
            else if (currentPlayerTemperature <= 5f)
            {
                postProcessingController.BlueAdjustmentUnder5();
            }
        
        }
    }

    

}
