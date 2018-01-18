using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_UIManager : MonoBehaviour
{
    private S_GameManager m_GameManagerScript;

    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        if(m_GameManagerScript.G_GameState == 2)
        {
            transform.Find("SecondsText").GetComponent<TMPro.TextMeshProUGUI>().text = "Time of Completion in Seconds: " + m_GameManagerScript.G_TimeOfGame + " seconds!";
        }
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
    