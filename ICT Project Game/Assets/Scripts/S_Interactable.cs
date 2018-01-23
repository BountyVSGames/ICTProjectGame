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

        [SerializeField] private Vector3 m_StartPosition;

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

            m_StartPosition = transform.position;

            m_PickUpDelay = 1;
            StartCoroutine(DelayTimer());
        }

        //Normal Update (Updated every frame)
        private void Update()
        {
            if(m_PickedUp)
            {
                Debug.Log(Input.GetMouseButtonDown(0));
                Debug.Log(m_PickUpDelay);
                Debug.Log(m_PcComponentHolderActive);
            }

            if (m_PickedUp && Input.GetMouseButtonDown(0) && m_PickUpDelay <= 0 && !m_PcComponentHolderActive)
            {
                Debug.Log("T");
                Disconnect();
            }
        }

        private void ComponentInteractWithComponentHolder(S_PcComponentHolder PcComponentHolderScript, GameObject Collide)
        {
            m_PcComponentHolderActive = true;

            if (!Collide.GetComponent<MeshRenderer>().enabled)
            {
                Collide.GetComponent<MeshRenderer>().enabled = true;

                PcComponentHolderScript.S_PcComponentScript = this;
            }

            if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
            {
                PcComponentHolderScript.GetComponent<MeshRenderer>().enabled = false;

                PcComponentHolderScript.enabled = false;

                Connect(Collide.gameObject, null, PcComponentHolderScript);
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
                            GetComponents<Collider>()[i].enabled = false;
                        }
                    }

                    m_PlayerScript = PlayerScript;

                    m_PickedUp = true;
                }
                else if (PlayerScript == null && PcComponentHolderScript != null)
                {
                    GetComponents<Collider>()[0].enabled = false;
                    GetComponents<Collider>()[1].enabled = false;

                    transform.rotation = transform.parent.rotation;

                    PcComponentHolderScript.G_PcBehaviourScript.G_BoolCheck[(int)m_Component] = true;
                    PcComponentHolderScript.G_PcBehaviourScript.UpdateChecklist();

                    m_PlayerScript.Disconnect();
                    m_PlayerScript = null;
                }

                m_PickUpDelay = 1;
                StartCoroutine(DelayTimer());
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

        void OnTriggerStay(Collider Collide)
        {
            if (Collide.gameObject != this.gameObject)
            {
                if(Collide.GetComponent<S_PcComponentHolder>() != null && Collide.GetComponent<S_PcComponentHolder>().G_Component == m_Component)
                {
                    S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

                    if ((m_Component != e_Components.Processor && m_Component != e_Components.ProcessorCooling && m_Component != e_Components.Ram && m_Component != e_Components.GraphicsCard))
                    {
                        ComponentInteractWithComponentHolder(PcComponentHolderScript, Collide.gameObject);
                    }
                    else if ((m_Component == e_Components.Processor || m_Component == e_Components.Ram || m_Component == e_Components.GraphicsCard) && !PcComponentHolderScript.G_PcBehaviourScript.G_BoolCheck[(int)e_Components.Motherboard] && m_Component != e_Components.ProcessorCooling)
                    {
                        ComponentInteractWithComponentHolder(PcComponentHolderScript, Collide.gameObject);
                    }
                    else if (m_Component == e_Components.ProcessorCooling && !PcComponentHolderScript.G_PcBehaviourScript.G_BoolCheck[(int)e_Components.Motherboard])
                    {
                        ComponentInteractWithComponentHolder(PcComponentHolderScript, Collide.gameObject);
                    }
                }
                else if(Collide.GetComponent<S_ComponentTrigger>())
                {
                    transform.position = m_StartPosition;
                }
            }
        }

        void OnTriggerExit(Collider Collide)
        {
            if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null)
            {
                S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

                PcComponentHolderScript.GetComponent<MeshRenderer>().enabled = false;

                m_PcComponentHolderActive = false;
            }
        }

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
