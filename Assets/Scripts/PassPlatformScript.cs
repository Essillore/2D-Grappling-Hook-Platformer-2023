using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlatformScript : MonoBehaviour
{

    public void Timerstart()
    {
        StartCoroutine(PassPlatformTimer());
    }

    public IEnumerator PassPlatformTimer()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

}
