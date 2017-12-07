using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class s_GameManager : MonoBehaviour
{
    private enum e_GameState
    {
        menu,
        game,
        gameOver
    }

    private e_GameState m_GameState;

    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(this.tag).Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        m_GameState = e_GameState.menu;
    }

    public void GoToScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
