using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetGoldChiliGear : MonoBehaviour
{
    public GameObject gm;
    public ChiliCollected chiliCollected;
    public GoldCollected goldCollected;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GM");
        if (gm != null)
        {

        chiliCollected = gm.GetComponent<ChiliCollected>();
        goldCollected = gm.GetComponent<GoldCollected>();

        goldCollected.ResetGoldCollected();
        chiliCollected.ResetChiliCollected();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
