using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject gm;
    public GoldCollected goldCounter;

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
            Destroy(gameObject);
        }
    }
}
