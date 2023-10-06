using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float transitionTime = 1f;

    void Start()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            SceneManager.LoadScene(3);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene(2);
        }

    }
    
    public void ChangeLevel(int levelNumber)
    {
        //pause.paused = false;
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

