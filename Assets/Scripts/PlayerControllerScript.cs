using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.VisualScripting;

public class PlayerControllerScript : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 6f;
    public float swingSpeed = 1f;
    public float horizontal;
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
    public GameObject playerSpriteObject;
    public SpriteRenderer[] playerSprite;
    public SpriteRenderer[] playerHat;
    public SpriteRenderer[] playerScarf;
    public Animator playerAnim;
    public bool isGrapplingA = false;
    public bool isRunningA = false;
    public bool animationsON = true;
    public SpriteRenderer iceBlock;
    private bool isFrozen = false;
    private bool inFreezingWater = false;

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
    public float ropeLengthSpeed = 3.0f;
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
        iceBlock.gameObject.SetActive(false);
        transform.position = playerSpawn.position;
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

        if (!isFrozen)
        {
            if (!gHook.isGrappling)
            {
                myRB.velocity = new Vector2(horizontal * speed, clampedYVel);
            }
        }
        if (isFrozen)
        {
            myRB.velocity = new Vector2(0, 0);
        }

        if (gHook.isGrappling)
        {
            if (movingHor) 
            {
                myRB.velocity = new Vector2(myRB.velocity.x + horizontal * swingSpeed, clampedYVel);
            }

            if(movingVert)
            {
                gHook.ropeDistance -= vertical * ropeLengthSpeed * Time.fixedDeltaTime;
                gHook.ropeDistance = Mathf.Max(gHook.ropeDistance, 1f);
            }
        }

        if (!onMovingPlat)
        {
            if (animationsON)
            {

            }
        }
    }

    void Update()
    {
        if (gHook.isGrappling)
        {
            if (!isGrapplingA) 
            {
            isGrapplingA = true;
            playerAnim.SetBool("isGrappling", true);
            }
        }

        if(movingHor || movingVert)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
        KeepingWarm();

        if (jumpBufferTimer > 0f && coyoteTime > 0f)
        {
            if (!isFrozen)
            playerAnim.SetTrigger("Jump");
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

        if (horizontal != 0f && !isRunningA)
        {
            isRunningA= true;
            playerAnim.SetBool("Running", true);
        }
        else if (horizontal == 0f)
        {
            isRunningA= false;
            playerAnim.SetBool("Running", false);
        }

        if (IsGrounded())
        {
            gHook.grappleStacks = 2f;

            //landing squeeze animation
            if (!hasLanded)
            {
                if (animationsON)
                {
                
                }
                hasLanded = true;
            }

            else if (hasLanded)
            {
                if (animationsON) 
                {
                
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

        Vector3 currentScale = playerSpriteObject.transform.localScale;
        if (!isFrozen)
        {
            if (facingRight)
            {
                currentScale.x = -1;
                playerSpriteObject.transform.localScale = currentScale;
                facingRight = false;
            }
            else if (!facingRight)
            {
                currentScale.x = 1;
                playerSpriteObject.transform.localScale = currentScale;
                facingRight = true;
            }
        }
    }

    public void ReturnToCheckPoint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StopCoroutine("OnDeath");
            gHook.StopGrapple();
            isFrozen = false;
            inFreezingWater = false;
            iceBlock.gameObject.SetActive(false);
            transform.position = playerSpawn.position;
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

        if (context.performed && gHook.isGrappling)
        {
            gHook.StopGrapple();
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
            playerTemperature.currentPlayerTemperature = playerTemperature.initialPlayerTemperature;
            playerSpawn.position = collision.transform.position;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Death();
        }
        if (collision.gameObject.CompareTag("FreezingWater"))
        {
            inFreezingWater = true;
            Death();
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
    }

    public void Death()
    {
        StartCoroutine("OnDeath");
        gHook.StopGrapple();
    }

    private IEnumerator OnDeath()
    {
        if (inFreezingWater)
        {
            playerTemperature.currentPlayerTemperature = 0f;
            Frozen();
            yield return new WaitForSeconds(2f);
        }
        isFrozen = false;
        inFreezingWater= false;
        iceBlock.gameObject.SetActive(false);
        transform.position = playerSpawn.position;
    }

    private void Frozen()
    {
        isFrozen= true;
        iceBlock.gameObject.SetActive(true);
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
