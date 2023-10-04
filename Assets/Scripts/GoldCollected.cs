using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoldCollected : MonoBehaviour
{
    public GoldCollected goldCollected;
    public int goldCollectedNumber;
    public TMP_Text goldCollectedUI;
    // Start is called before the first frame update
    void Start()
    {
        FindGoldUI();
    }

    public void FindGoldUI()
    {
        goldCollectedUI = GameObject.Find("GoldUI").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoldIsCollected()
    {
        goldCollectedNumber++;
        goldCollectedUI.text = " " + goldCollectedNumber;
    }

    public void ResetGoldCollected()
    {
        goldCollectedNumber = 0;
    }

}
