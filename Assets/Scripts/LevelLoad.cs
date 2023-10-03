using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoad : MonoBehaviour
{
    public GameObject gm;
    public ChiliCollected chiliCollected;
    public GoldCollected goldCollected;
    public CameraController camController;
    // Start is called before the first frame update
    void Start()
    {
        //Inform CameraController to find player-> CameraTarget
        camController = GameObject.Find("CameraController").GetComponent<CameraController>();
        camController.FindPlayerAtSceneLoad();

        gm = GameObject.Find("GM");

        //Inform ChiliCollected to find the UI
        chiliCollected = gm.GetComponent<ChiliCollected>();
        chiliCollected.FindChiliUI();

        //Inform GoldCollected to find the UI
        goldCollected = gm.GetComponent<GoldCollected>();
        goldCollected.FindGoldUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
