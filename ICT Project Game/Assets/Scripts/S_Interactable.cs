using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICTProjectGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class S_Interactable : MonoBehaviour
    {
        [Header("Pick Up Settings")]
        [SerializeField] private bool m_PickedUp;
        [SerializeField] private float m_PickUpDelay;
        [SerializeField] private float m_TrowFroce;

        [SerializeField] private bool m_PcComponentHolderActive;

        [Space(5)]
        [Header("Component Settings")]
        [SerializeField] private e_Components m_Component;

        private GameObject m_PickedUpObject;

        private S_Player m_PlayerScript;

        private Rigidbody m_Rigidbody;

        public bool G_PcComponentHolderActive
        {
            get { return m_PcComponentHolderActive; }
        }
        public bool G_PickedUp
        {
            get { return m_PickedUp; }
        }

        //Initializing variable's
        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }

        //Normal Update (Updated every frame)
        private void Update()
        {
            if (m_PickedUp && Input.GetMouseButtonDown(0) && m_PickUpDelay <= 0)
            {
                Disconnect();
            }
        }

        #region Connecting and Disconnecting Objects for 'hand'
            public void Connect(GameObject ConnectToObject, S_Player PlayerScript = null, S_PcComponentHolder PcComponentHolderScript = null)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                transform.parent = ConnectToObject.transform;
                transform.localPosition = Vector3.zero;

                m_PickedUpObject = ConnectToObject;

                if (PlayerScript != null && PcComponentHolderScript == null)
                {
                    for (int i = 0; i < GetComponents<Collider>().Length; i++)
                    {
                        if (!GetComponents<Collider>()[i].isTrigger)
                        {
                            GetComponents<Collider>()[i].enabled = true;
                        }
                    }

                    m_PlayerScript = PlayerScript;

                    m_PickedUp = true;
                }
                else if (PlayerScript == null && PcComponentHolderScript != null)
                {
                    GetComponents<Collider>()[0].enabled = false;
                    GetComponents<Collider>()[1].enabled = true;

                    PcComponentHolderScript.G_PcBehaviourScript.G_BoolCheck[(int)m_Component] = true;
                }

                m_PickUpDelay = 1;
            }
            public void Disconnect()
            {
                GameObject OldConnectedObject = transform.parent.gameObject;
                Rigidbody ObjectsRigidbody = GetComponent<Rigidbody>();

                transform.parent = null;

                ObjectsRigidbody.constraints = RigidbodyConstraints.None;

                for (int i = 0; i < GetComponents<Collider>().Length; i++)
                {
                    if(!GetComponents<Collider>()[i].isTrigger)
                    {
                        GetComponents<Collider>()[i].enabled = true;
                    }
                }

                if(m_PlayerScript != null)
                {
                    ObjectsRigidbody.AddForce(m_PickedUpObject.transform.forward * m_TrowFroce, ForceMode.Impulse);

                    m_PlayerScript.Disconnect();
                }

                m_PlayerScript = null;
                m_PickedUpObject = null;
                m_PickedUp = false;
            }
        #endregion

        private IEnumerator DelayTimer()
        {
            while (m_PickUpDelay > 0)
            {
                m_PickUpDelay -= 0.25f;
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
