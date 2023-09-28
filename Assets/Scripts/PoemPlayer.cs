using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoemPlayer : MonoBehaviour
{
    public AudioSource m_poemAtLocation;
    public bool m_ToggleChange;

    // Start is called before the first frame update
    void Start()
    {
        m_poemAtLocation = GetComponent <AudioSource>();
        m_ToggleChange = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && m_ToggleChange)
            {
            m_poemAtLocation.Play();
            m_ToggleChange = false;
            }
    }
}
