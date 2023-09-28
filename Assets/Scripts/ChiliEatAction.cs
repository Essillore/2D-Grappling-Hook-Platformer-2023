using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChiliEatAction : MonoBehaviour
{
    public ChiliCollected chiliCollected;


    // Start is called before the first frame update
    void Start()
    {
        chiliCollected = GameObject.Find("GM").GetComponent<ChiliCollected>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EatAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            chiliCollected.ChiliEaten();
        }
    }

}
