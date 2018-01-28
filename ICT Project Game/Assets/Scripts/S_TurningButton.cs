using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICTProjectGame.Player
{
    public class S_TurningButton : MonoBehaviour
    {
        private S_Player m_Player;

        private void Start()
        {
            m_Player = GameObject.FindObjectOfType<S_Player>();
        }

        private void Update()
        {
            transform.GetChild(0).eulerAngles += new Vector3(0, 1f, 1f) * (Vector3.Distance(transform.position, m_Player.transform.position) * 10) * Time.deltaTime;
        }
    }

}