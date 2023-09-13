using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 6f;
    private float horizontal;
    private float vertical;
    public float jumpForce = 10f;
    public float maxYVelocity;
    public Rigidbody2D myRB;
    public Animator stretchAnimator;
    public bool facingRight = true;
    private bool hasLanded = false;

    [Header("Grounded")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    [Header("Death Values")]
    public Transform playerSpawn;

    [Header("PhysicMats")]
    public Collider2D myCol;
    public PhysicsMaterial2D slide;
    public PhysicsMaterial2D stop;
    public bool onMovingPlat = false;
    public bool onPassPlat = false;
    public bool onRopeGrabRange = false;
    public GameObject activePassPlat;
    public PassPlatformScript platScript;

    [Header("CoyoteTime & Jump Buffer")]
    public float coyoteTime = 0.3f;
    public float coyoteTimeTimer;
    public float jumpBuffer = 0.3f;
    public float jumpBufferTimer;

    //Riikan lisäyksiä
    [Header("Temperature")]
    public TemperatureTimer temperature;


    void Start()
    {
        transform.position = playerSpawn.position;
        stretchAnimator = GetComponentInChildren<Animator>();
        temperature = GetComponent<TemperatureTimer>();
    }

    //Check if player is grounded
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        //clamp maximum speed for player incase of funky physics
        float clampedYVel = Mathf.Clamp(myRB.velocity.y, -maxYVelocity, maxYVelocity);

        myRB.velocity = new Vector2(horizontal * speed, clampedYVel);

        if (!onMovingPlat)
        {
            stretchAnimator.SetFloat("yVelocity", myRB.velocity.y);
        }
    }

    void Update()
    {
        if (jumpBufferTimer > 0f && coyoteTime > 0f)
        {
            myRB.AddForce(Vector2.up * jumpForce);
            jumpBufferTimer = 0f;
        }

        //Moving platform braking material swap
        if (horizontal != 0f && onMovingPlat)
        {
            myCol.sharedMaterial = slide;
        }
        if (horizontal == 0f && onMovingPlat)
        {
            myCol.sharedMaterial = stop;
        }

        //swap facing direction
        if (horizontal < -0.1f && facingRight)
        {
            Flip();
        }

        if (horizontal > 0.1f && !facingRight)
        {
            Flip();
        }

        /*//animate running
        if (horizontal != 0f)
        {
            myAnim.SetBool("IsRunning", true);
        }
        else if (horizontal == 0f)
        {
            myAnim.SetBool("IsRunning", false);
        }*/

        if (IsGrounded())
        {
            //landing squeeze animation
            if (!hasLanded)
            {
                stretchAnimator.SetBool("Squeeze", true);
                hasLanded = true;
            }

            else if (hasLanded)
            {
                stretchAnimator.SetBool("Squeeze", false);
            }

            coyoteTimeTimer = coyoteTime;
            //myAnim.SetBool("isGrounded", true);
        }
        else
        {
            hasLanded = false;
            coyoteTimeTimer -= Time.deltaTime;
            //myAnim.SetBool("isGrounded", false);
        }

        //Deactivate pass platform collider
        if (vertical < -0.1f && onPassPlat)
        {
            platScript = activePassPlat.GetComponent<PassPlatformScript>();
            platScript.Timerstart();
        }

    }

    public void Flip()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        facingRight = !facingRight;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimeTimer > 0f)
        {
            jumpBufferTimer = jumpBuffer;
            KeepingWarm(3f);
        }

        if (context.canceled && myRB.velocity.y > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
            myRB.velocity = new Vector2(myRB.velocity.x, myRB.velocity.y * 0.6f);
            coyoteTimeTimer = 0f;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //screenshake for camera incase of death
            //camControl.ScreenShake(10f, 0.3f, 10f);
            transform.position = playerSpawn.position;
        }
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            onMovingPlat = true;
            myCol.sharedMaterial = stop;
        }

        if (collision.gameObject.CompareTag("PassPlatform"))
        {
            onPassPlat = true;
            activePassPlat = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            onMovingPlat = false;
            myCol.sharedMaterial = slide;
        }

        if (collision.gameObject.CompareTag("PassPlatform"))
        {
            onPassPlat = false;
            activePassPlat = null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CheckPoint"))
        {
            playerSpawn.position = collision.transform.position;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            transform.position = playerSpawn.position;
        }

        if (collision.gameObject.CompareTag("Rope"))
        {
            onRopeGrabRange = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            onRopeGrabRange = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;
        KeepingWarm(2f);
    }

    //If player moves, jumps (or grapples), add to timer to stop character temperature from dropping.
    public void KeepingWarm(float howLongTimer)
    {
        temperature.timer = 0.1f + howLongTimer;
    }

}
