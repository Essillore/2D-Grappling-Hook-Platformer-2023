using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ChiliCollected : MonoBehaviour
{
    public ChiliCollected chiliCollected;
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
        FindChiliUI();
    }

    private void Awake()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindChiliUI()
    {
        playerTemperature = GameObject.Find("Player").GetComponent<PlayerTemperature>();
        chiliStoredUI = GameObject.Find("ChiliUI").GetComponent<TMP_Text>();
    }

    public void ChiliIsCollected()
    {
        chiliStoredNumber++;
        chiliCollectedNumber++;
        chiliStoredUI.text = chiliStoredNumber + "  ";
    }

    public void ChiliEaten()
    {
        print("Chili eaten");
        if (chiliStoredNumber >= 1)
        {
            playerTemperature.currentPlayerTemperature += chiliWarmingAmount;
            chiliStoredNumber--;
            chiliStoredUI.text = chiliStoredNumber + "  ";
        }
    }

}
