using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public float hookSpeed = 10f;
    public float maxDistance = 10f;
    public float swingStrength = 10f;
    public LayerMask hookableLayers;
    public SpringJoint2D springJoint;

    public bool isGrappling = false;
    private Vector2 grapplePoint;
    private Vector2 startPoint;
    private Rigidbody2D rb;
    private float ropeDistance;

    //[Header("Physics")]



    void Start()
    {
        springJoint.enabled = false;
        rb= GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        startPoint= transform.position;

        if (isGrappling)
        {

            //LineRenderer
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, grapplePoint);
        }
    }

    public void Hook(InputAction.CallbackContext context)
    {
        if (context.performed & !isGrappling)
        {
            StartGrapple();
        }
        else if(context.performed && isGrappling)
        {
            StopGrapple();
        }
    }

    private void StartGrapple()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, hookableLayers);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            isGrappling=true;
            ropeDistance= Vector2.Distance(transform.position, grapplePoint);
            StartCoroutine(Grapple());
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;
        springJoint.enabled = false;
        rb.gravityScale = 6f;
        rb.rotation = 0f;
        rb.freezeRotation = true;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

    }

    private IEnumerator Grapple()
    {
        while(isGrappling)
        {
            rb.freezeRotation = false;
            rb.angularVelocity *= 0.5f;
            rb.gravityScale = 20f;
            springJoint.enabled = true;
            springJoint.distance = ropeDistance - 1f;
            springJoint.connectedAnchor = grapplePoint;
            yield return null;
        }
    }
}
