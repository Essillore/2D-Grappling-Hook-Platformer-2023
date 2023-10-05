using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager instance;

    public static bool atmosphericTemperature;
    public static bool infiniteGrappleStacks;
    // Start is called before the first frame update

        private void Awake()
        {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Keycode to change temperature to atmospheric or not midgame
        if (Input.GetKeyDown(KeyCode.L))
            {
                ToggleAtmosphericTemperature();
            }


    }

    public void ToggleInfiniteGrapplestacks()
    {
        infiniteGrappleStacks = !infiniteGrappleStacks;
    }

    public void ToggleAtmosphericTemperature()
    {
        
        // Button to toggle Temperature to atmoshperic or killing boolean
        atmosphericTemperature = !atmosphericTemperature;
    }
}
