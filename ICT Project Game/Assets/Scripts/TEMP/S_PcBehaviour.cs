using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcBehaviour : MonoBehaviour
{
    [SerializeField]
    private bool[] m_BoolCheck;

    private S_GameManager m_GameManagerScript;

    public bool[] G_BoolCheck
    {
        get { return m_BoolCheck; }
    }

    private void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_BoolCheck = new bool[System.Enum.GetValues((typeof(e_Components))).Length];
    }

    public void CheckWin()
    {
        for (int i = 0; i < m_BoolCheck.Length; i++)
        {
            if(!m_BoolCheck[i])
            {
                break;
            }
            else if(m_BoolCheck[i] && (i == m_BoolCheck.Length - 1))
            {
                m_GameManagerScript.GoToScene("GameOver");
            }
        }
    }
}
