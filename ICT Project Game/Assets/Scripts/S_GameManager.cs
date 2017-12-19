using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{
    private enum e_GameState
    {
        menu,
        game,
        gameOver
    }

    private e_GameState m_GameState;
    [SerializeField]
    private float m_MouseSensitivity;

    public float G_MouseSensitivity
    {
        get { return m_MouseSensitivity; }
    }

    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(this.tag).Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        m_GameState = e_GameState.menu;

        m_MouseSensitivity = 2f;
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
