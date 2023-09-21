using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chili : MonoBehaviour
{

    public GameObject gm;
    public ChiliCollected chiliCounter;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GM");
        chiliCounter = gm.GetComponent<ChiliCollected>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            chiliCounter.ChiliIsCollected();
            Destroy(gameObject);
        }
    }



}
