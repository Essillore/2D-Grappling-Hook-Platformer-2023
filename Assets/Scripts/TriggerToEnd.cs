using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerToEnd : MonoBehaviour
{
    public LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                levelManager.ChangeLevel(2);
            }
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                levelManager.ChangeLevel(5);
            }
            
        }
    }
}
