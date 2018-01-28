using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICTProjectGame.Player
{
    public class S_PcBehaviour : MonoBehaviour
    {
        [SerializeField]
        private bool[] m_BoolCheck;

        private ICTProjectGame.Managment.S_GameManager m_GameManagerScript;
        private ICTProjectGame.Managment.S_Checklist m_ChecklistScript;

        private GameObject m_Monitor;

        [SerializeField]
        private Material[] m_MonitorScreens;

        public bool[] G_BoolCheck
        {
            get { return m_BoolCheck; }
        }

        private void Start()
        {
            m_GameManagerScript = GameObject.FindObjectOfType<ICTProjectGame.Managment.S_GameManager>();
            m_ChecklistScript = GameObject.FindObjectOfType<ICTProjectGame.Managment.S_Checklist>();

            m_Monitor = GameObject.FindWithTag("Monitor");

            m_BoolCheck = new bool[System.Enum.GetValues((typeof(e_Components))).Length];
        }

        public void UpdateChecklist()
        {
            m_ChecklistScript.UpdateChecklist();
        }

        public void CheckWin()
        {
            for (int i = 0; i < m_BoolCheck.Length; i++)
            {
                if (!m_BoolCheck[i])
                {
                    Debug.Log("'LOSE'");
                    m_Monitor.GetComponent<MeshRenderer>().material = m_MonitorScreens[1];
                    StartCoroutine(GoToEndScreen(false));
                    break;
                }
                else if (m_BoolCheck[i] && (i == m_BoolCheck.Length - 1))
                {
                    //m_GameManagerScript.GoToScene("GameOver");
                    Debug.Log("'WIN'");
                    m_Monitor.GetComponent<MeshRenderer>().material = m_MonitorScreens[0];
                    StartCoroutine(GoToEndScreen(true));
                }
            }
        }

        private IEnumerator GoToEndScreen(bool WinState)
        {
            yield return new WaitForSeconds(5f);
            if(WinState)
            {
                m_GameManagerScript.GoToScene("GameOver");
            }
            else
            {
                m_GameManagerScript.GoToScene("MainMenu");
            }
        }
    }

}