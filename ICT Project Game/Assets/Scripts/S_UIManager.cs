using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UIManager : MonoBehaviour
{
    private s_GameManager m_GameManagerScript;

    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<s_GameManager>();
    }

    public void ChangeScene(string Scene)
    {
        m_GameManagerScript.GoToScene(Scene);
    }
    public void QuitGame()
    {
        m_GameManagerScript.QuitGame();
    }
}
