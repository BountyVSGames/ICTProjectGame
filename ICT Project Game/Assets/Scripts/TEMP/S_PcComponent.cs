using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class S_PcComponent : MonoBehaviour
{
    [Header("Component Selecting")]
    [Tooltip("Select the component this has to be here")]
    [SerializeField]
    private e_Components m_Component;

    private S_GameManager m_GameManagerScript;
    private S_CameraFollowsMouse m_PlayerScript;

    [Space(5)]
    [Header("Debug Information:")]
    [SerializeField]
    private bool m_PickedUp;
    private bool m_PcComponentHolderActive;

    private float m_MouseSensitivity;

    public bool G_PickedUp
    {
        get { return m_PickedUp; }
    }
    public bool G_PcComponentHolderActive
    {
        get { return m_PcComponentHolderActive; }
    }

    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;

        m_PcComponentHolderActive = false;
    }

    public void Connect(GameObject GameObjectToPlugWith, GameObject ComponentHolder = null, S_CameraFollowsMouse PlayerScript = null)
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
        {
            BoxCollider ComponentBoxCollider = GetComponents<BoxCollider>()[i];

            if (!ComponentBoxCollider.isTrigger)
            {
                ComponentBoxCollider.enabled = false;
                break;
            }
        }

        if(ComponentHolder != null && PlayerScript != null)
        {
            transform.parent = ComponentHolder.transform;
            transform.localPosition = Vector3.zero;

            m_PickedUp = true;

            m_PlayerScript = PlayerScript;
        }
        else if(ComponentHolder != null && PlayerScript == null)
        {
            Debug.LogError("Add Playerscript to the parameters!");
        }
        else if(ComponentHolder == null && PlayerScript != null)
        {
            Debug.LogError("Add ComponentHolder to the parameters!");
        }
        else if(GameObjectToPlugWith != null && m_PlayerScript != null)
        {
            transform.parent = GameObjectToPlugWith.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);

            m_PlayerScript.S_HoldingComponent = null;
            m_PlayerScript.S_PcComponentScript = null;

            m_PlayerScript = null;

            Debug.Log("Test");

            this.enabled = false;
        }
    }

    public void Disconnect()
    {
        transform.parent = null;

        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
        {
            BoxCollider ComponentBoxCollider = GetComponents<BoxCollider>()[i];

            if (!ComponentBoxCollider.isTrigger)
            {
                ComponentBoxCollider.enabled = true;
                break;
            }
        }

        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    void OnTriggerStay(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null && Collide.GetComponent<S_PcComponentHolder>().G_Component == m_Component)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            m_PcComponentHolderActive = true;

            if (!Collide.GetComponent<MeshRenderer>().enabled)
            {
                Collide.GetComponent<MeshRenderer>().enabled = true;

                PcComponentHolderScript.S_PcComponentScript = this;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                Collide.GetComponent<MeshRenderer>().enabled = false;

                PcComponentHolderScript.enabled = false;

                Connect(Collide.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            PcComponentHolderScript.G_MeshRenderer.enabled = false;

            m_PcComponentHolderActive = false;
        }
    }
}
