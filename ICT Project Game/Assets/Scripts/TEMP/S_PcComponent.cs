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

    [Space(5)]
    [Header("Debug Information:")]
    [SerializeField]
    private bool m_PickedUp;
    private float m_MouseSensitivity;

    public bool GS_PickedUp
    {
        get { return m_PickedUp; }
        set { m_PickedUp = value; }
    }

    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;
    }

    void Update()
    {
        if(m_PickedUp && Input.GetMouseButton(1))
        {
            float MouseX = Input.GetAxis("Mouse X") * (m_MouseSensitivity * 70);
            float MouseY = Input.GetAxis("Mouse Y") * (m_MouseSensitivity * 70);

            transform.localEulerAngles += new Vector3(MouseY, -MouseX, 0) * Time.deltaTime;
        }
    }

    void OnTriggerStay(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null && Collide.GetComponent<S_PcComponentHolder>().G_Component == m_Component)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            if(!PcComponentHolderScript.G_MeshRenderer.enabled)
            {
                PcComponentHolderScript.G_MeshRenderer.enabled = true;
                PcComponentHolderScript.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;

                PcComponentHolderScript.transform.localScale = transform.localScale;

                PcComponentHolderScript.S_PcComponentScript = this;
            }
        }
    }

    void OnTriggerExit(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            PcComponentHolderScript.G_MeshRenderer.enabled = false;
        }
    }
}
