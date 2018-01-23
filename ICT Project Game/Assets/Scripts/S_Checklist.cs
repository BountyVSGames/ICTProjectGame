using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICTProjectGame.Managment
{
    public class S_Checklist : MonoBehaviour
    {
        [SerializeField]
        private bool[] m_BoolCheck;
        [SerializeField]
        private TMPro.TextMeshPro[] m_UI;

        private ICTProjectGame.Player.S_PcBehaviour m_PCBehaviourScript;

        private void Awake()
        {
            m_UI = GetComponentsInChildren<TMPro.TextMeshPro>();
        }

        private void Start()
        {
            m_PCBehaviourScript = GameObject.FindObjectOfType<ICTProjectGame.Player.S_PcBehaviour>();
        }

        public void UpdateChecklist()
        {
            bool[] Checklist = m_PCBehaviourScript.G_BoolCheck;

            for(int i = 0; i < Checklist.Length; i++)
            {
                if(Checklist[i])
                {
                    m_UI[i + 1].fontStyle = TMPro.FontStyles.Strikethrough;
                    continue;
                }
            }
        }
    }
}