using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    public GameObject gm;
    public ChiliCollected chiliCollected;
    public GoldCollected goldCollected;
    public CameraController camController;
    public PlayerTemperature playerTemperature;
    public GrapplingHook grapplingHook;
    public GameModeManager gameModeManager;

    // Start is called before the first frame update
    void Start()
    {
        //Inform CameraController to find player-> CameraTarget
        camController = GameObject.Find("CameraController").GetComponent<CameraController>();
        camController.FindPlayerAtSceneLoad();

        gm = GameObject.Find("GM");

        //Inform ChiliCollected to find the UI
        chiliCollected = GameObject.Find("GM").GetComponent<ChiliCollected>();
        chiliCollected.FindChiliUI();

        //Inform GoldCollected to find the UI
        goldCollected = GameObject.Find("GM").GetComponent<GoldCollected>();
        goldCollected.FindGoldUI();

        playerTemperature = GameObject.Find("Player").GetComponent<PlayerTemperature>();
        playerTemperature.FindTemperatureUI();


        grapplingHook = GameObject.Find("GrapplingHook").GetComponent<GrapplingHook>();
        gameModeManager = GameObject.Find("LevelLoad").GetComponent<GameModeManager>();
        grapplingHook.InfiniteGrappleIsOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
