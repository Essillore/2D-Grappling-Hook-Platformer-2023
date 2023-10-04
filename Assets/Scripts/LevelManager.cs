using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //public PauseScript pause;
    // public Animator fadeScreen;
    public float transitionTime = 1f;

    void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(3);
        }

        //  pause = GetComponent<PauseScript>();
        //  fadeScreen = GameObject.Find("FadeScreen").GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(3);
            }

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SceneManager.LoadScene(2);
        }

    }
    
    public void ChangeLevel(int levelNumber)
    {
        //pause.paused = false;
        // fadeScreen.SetTrigger("ChangeLevel");
        StartCoroutine(NewLevel(levelNumber));
    }

    public IEnumerator NewLevel(int levelNumber)
    {
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelNumber);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

}

