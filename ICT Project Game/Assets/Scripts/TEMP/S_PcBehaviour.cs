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

        public bool[] G_BoolCheck
        {
            get { return m_BoolCheck; }
        }

        private void Start()
        {
            m_GameManagerScript = GameObject.FindObjectOfType<ICTProjectGame.Managment.S_GameManager>();
            m_ChecklistScript = GameObject.FindObjectOfType<ICTProjectGame.Managment.S_Checklist>();

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
                    break;
                }
                else if (m_BoolCheck[i] && (i == m_BoolCheck.Length - 1))
                {
                    m_GameManagerScript.GoToScene("GameOver");
                }
            }
        }
    }

}