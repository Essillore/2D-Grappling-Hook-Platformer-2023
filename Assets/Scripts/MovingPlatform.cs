using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D myRB;
    public float speedX = 3f;
    public float speedY = 0f;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {

        myRB.velocity = new Vector2(speedX, speedY);

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            speedX = -speedX;
            speedY = -speedY;
        }
    }
}
