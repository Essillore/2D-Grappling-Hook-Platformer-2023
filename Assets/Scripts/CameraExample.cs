using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExample : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public float delay = 1;

    // Start is called before the first frame update
    void Start()
    {
      // transform.LookAt(target);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        transform.position = Vector3.Lerp(transform.position, target.position, delay * Time.deltaTime);
    }
}
