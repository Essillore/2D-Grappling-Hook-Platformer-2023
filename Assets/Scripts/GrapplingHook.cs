using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public float hookSpeed = 10f;
    public float maxDistance = 10f;
    public float swingStrength = 10f;
    public LayerMask hookableLayers;
    public SpringJoint2D springJoint;
    public float grappleStacks = 2f;
    public TextMeshProUGUI grappleStacksTEXT;
    public bool isGrappling = false;
    private Vector2 grapplePoint;
    private Vector2 startPoint;
    private Vector2 rangeEndPoint;
    private float rangeMaxDistance;

    public bool rangeIndicatorON = true;
    private LineRenderer rangeIndicatorLine;

    [Header("Physics")]
    private Rigidbody2D rb;
    public float ropeDistance;


    void Start()
    {
        springJoint.enabled = false;
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rangeIndicatorLine = GameObject.Find("Player").GetComponent<LineRenderer>();
        rb.freezeRotation = true;
        rangeMaxDistance = maxDistance;
    }

    // Update is called once per frame
    void Update()
    {
        startPoint = transform.position;

        rangeEndPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        grappleStacksTEXT.text = "" + grappleStacks;

        if (isGrappling)
        {

            //LineRenderer
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, grapplePoint);
        }

        Vector2 direction = rangeEndPoint - startPoint;

        if (direction.magnitude > rangeMaxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        if (rangeIndicatorON)
        {
            rangeIndicatorLine.positionCount= 2;
            rangeIndicatorLine.SetPosition(0, startPoint);
            rangeIndicatorLine.SetPosition(1, startPoint + direction);
        }



    }

    public void Hook(InputAction.CallbackContext context)
    {
        if (context.performed & !isGrappling)
        {
            StartGrapple();
        }
        else if (context.performed && isGrappling)
        {
            StopGrapple();
        }
    }

    private void StartGrapple()
    {
        if (grappleStacks >= 1)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance, hookableLayers);

            if (hit.collider != null)
            {
                grapplePoint = hit.point;
                isGrappling = true;
                ropeDistance = Vector2.Distance(transform.position, grapplePoint);
                StartCoroutine(Grapple());
            }
        }
    }

    public void StopGrapple()
    {
        grappleStacks--;
        springJoint.enabled = false;
        rb.gravityScale = 6f;
        rb.velocity = new Vector2(rb.velocity.x * 10f, rb.velocity.y + 6f);
        rb.rotation = 0f;
        rb.freezeRotation = true;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        isGrappling = false;
    }

    private IEnumerator Grapple()
    {
        while (isGrappling)
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
