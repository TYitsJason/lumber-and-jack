using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public List<Animator> animators;
    public bool isPaused;

    public PlayerController cref;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Pause();
                
            }
            else
            {
                Resume();
                
            }
        }
    }
    public bool IsPaused()
    {
        return isPaused;
    }
    public void Resume()
    {
        isPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        foreach (Animator anim in animators)
        {
            anim.speed = 1;
        }
    }

    public void Pause()
    {
        isPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        foreach (Animator anim in animators)
        {
            anim.speed = 0;
        }
    }

    public void Load()
    {
        GameManager.Instance.LoadGame();
        cref.LoadGameComplete();
        GameManager.Instance.LoadGameComplete();
    }

    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene("Menu");
    }
}