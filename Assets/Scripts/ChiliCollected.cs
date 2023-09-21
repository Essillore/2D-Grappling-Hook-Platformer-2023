using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChiliCollected : MonoBehaviour
{
    //How many chilis player currently has
    public int chiliStoredNumber;
    public TMP_Text chiliStoredUI;
    //How many chilis player has collected totally
    public int chiliCollectedNumber;

    public PlayerTemperature playerTemperature;

    public float chiliWarmingAmount = 3f;

    // Start is called before the first frame update
    void Start()
    {
        playerTemperature = GameObject.Find("Player").GetComponent<PlayerTemperature>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChiliIsCollected()
    {
        chiliStoredNumber++;
        chiliCollectedNumber++;
        chiliStoredUI.text = chiliStoredNumber + "  ";
    }

    public void ChiliEaten()
    {
        if (chiliStoredNumber >= 1)
        {
            playerTemperature.currentPlayerTemperature =+ chiliWarmingAmount;
            chiliStoredNumber--;
            chiliStoredUI.text = chiliStoredNumber + "  ";
        }
    }

}
