using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GoldCollected : MonoBehaviour
{
    public int goldCollectedNumber;
    public TMP_Text goldCollectedUI;
    // Start is called before the first frame update
    void Start()
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


}
