using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 6f;
    public float swingSpeed = 1f;
    private float horizontal;
    private float vertical;
    public float jumpForce = 10f;
    public float maxYVelocity;
    public Rigidbody2D myRB;
    public bool facingRight = true;
    private bool hasLanded = false;
    private bool moving= false;
    private bool movingHor = false;
    private bool movingVert = false;

    [Header("Animation")]
    public SpriteRenderer playerSprite;
    public bool animationsON = true;
    public Animator stretchAnimator;

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
    public GameObject activePassPlat;
    public PassPlatformScript platScript;
    public GrapplingHook gHook;

    [Header("CoyoteTime & Jump Buffer")]
    public float coyoteTime = 0.3f;
    public float coyoteTimeTimer;
    public float jumpBuffer = 0.3f;
    public float jumpBufferTimer;

    //Riikan lisäyksiä
    [Header("Temperature")]
    //public TemperatureTimer temperature;
    public PlayerTemperature playerTemperature;


    void Start()
    {
        transform.position = playerSpawn.position;
        stretchAnimator = GetComponentInChildren<Animator>();
        playerTemperature = GetComponent<PlayerTemperature>();
        gHook= GameObject.Find("GrapplingHook").GetComponent<GrapplingHook>();
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

        if (!gHook.isGrappling)
        {
            myRB.velocity = new Vector2(horizontal * speed, clampedYVel);
        }

        if (gHook.isGrappling)
        {
            if (movingHor) 
            {
                myRB.velocity = new Vector2(myRB.velocity.x + horizontal * swingSpeed, clampedYVel);
            }

            if(movingVert)
            {
                gHook.ropeDistance = 0.1f;
            }
        }

        if (!onMovingPlat)
        {
            if (animationsON)
            {
                stretchAnimator.SetFloat("yVelocity", myRB.velocity.y);
            }
        }
    }

    void Update()
    {
        if(movingHor || movingVert)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

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
                if (animationsON)
                {
                    stretchAnimator.SetBool("Squeeze", true);
                }
                hasLanded = true;
            }

            else if (hasLanded)
            {
                if (animationsON) 
                {
                stretchAnimator.SetBool("Squeeze", false);
                }
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
        if (facingRight)
        {
            playerSprite.flipX = true;
            facingRight = false;
        }
        else if (!facingRight)
        {
            playerSprite.flipX = false;
            facingRight = true;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && coyoteTimeTimer > 0f)
        {
            jumpBufferTimer = jumpBuffer;
            KeepingWarm();
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
    }

    public void OnTriggerExit2D(Collider2D collision)
    {

    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
        vertical = context.ReadValue<Vector2>().y;

        movingHor = Mathf.Abs(horizontal) > 0.1f && context.action.triggered;
        movingVert = Mathf.Abs(vertical) > 0.1f && context.action.triggered;

        KeepingWarm();
    }

    //Pass information to PlayerTemperature about if player is moving or not
    public void KeepingWarm()
    {
       // temperature.timer = 0.1f + howLongTimer;

        if (moving == true)
        {
            MovingToGetWarmer();
        }
        else if (moving == false)
        {
            IsStillGettingCooler();
        }
    }
    public void MovingToGetWarmer()
    {
        playerTemperature.isMoving = true;
    }

    public void IsStillGettingCooler()
    {
        playerTemperature.isMoving = false;
    }
}
