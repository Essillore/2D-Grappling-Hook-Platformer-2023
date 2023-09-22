using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject gm;
    public GoldCollected goldCounter;
    //public GameObject pickUp;

    private void Start()
    {
        gm = GameObject.Find("GM");
        goldCounter = gm.GetComponent<GoldCollected>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if
        (collision.CompareTag("Player"))
        {
            goldCounter.GoldIsCollected();
           // Instantiate(pickUp, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
