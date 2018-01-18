using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{
    private enum e_GameState
    {
        Menu,
        Game,
        GameOver
    }

    private e_GameState m_GameState;
    [SerializeField]
    private float m_MouseSensitivity;
    private float m_TimeOfGame;

    private bool m_ShowMouse;

    public int G_GameState
    {
        get { return (int)m_GameState; }
    }

    public float G_MouseSensitivity
    {
        get { return m_MouseSensitivity; }
    }

    public float G_TimeOfGame
    {
        get { return m_TimeOfGame; }
    }


    void Awake()
    {
        if(GameObject.FindGameObjectsWithTag(this.tag).Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        m_MouseSensitivity = 2f;
    }

    void Start()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Game":
                m_GameState = e_GameState.Game;

                Cursor.lockState = CursorLockMode.Locked;

                m_ShowMouse = false;
                break;
            case "GameOver":
                m_GameState = e_GameState.GameOver;

                Cursor.lockState = CursorLockMode.None;

                m_ShowMouse = true;
                break;
            case "MainMenu":
                m_GameState = e_GameState.Menu;

                Cursor.lockState = CursorLockMode.Locked;

                m_ShowMouse = false;
                break;
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && m_GameState != e_GameState.GameOver)
        {
            m_ShowMouse = !m_ShowMouse;
        }
        Cursor.visible = m_ShowMouse;

        if(Input.GetKeyDown(KeyCode.R))
        {
            GoToScene("MainMenu");
        }

        switch (m_ShowMouse)
        {
            case false:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case true:
                Cursor.lockState = CursorLockMode.None;
                break;
            default:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    public void GoToScene(string Scene)
    {
        switch (Scene)
        {
            case "Game":
                m_GameState = e_GameState.Game;

                Cursor.lockState = CursorLockMode.Locked;

                m_ShowMouse = false;
                break;
            case "GameOver":
                m_GameState = e_GameState.GameOver;

                m_TimeOfGame = Time.timeSinceLevelLoad;

                Cursor.lockState = CursorLockMode.None;

                m_ShowMouse = true;
                break;
            case "MainMenu":
                m_GameState = e_GameState.Menu;

                Cursor.lockState = CursorLockMode.None;

                m_ShowMouse = true;
                break;
        }

        SceneManager.LoadScene(Scene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
