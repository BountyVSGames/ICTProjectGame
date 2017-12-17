using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class S_PcComponent : MonoBehaviour
{
    [SerializeField]
    private e_Components m_Component;

    private S_GameManager m_GameManagerScript;

    private Bounds m_Bounds;
    private BoxCollider m_Collider;

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

        for (int i = 0; i < GetComponents<BoxCollider>().Length; i++)
        {
            BoxCollider ComponentBoxCollider = GetComponents<BoxCollider>()[i];
            if(!ComponentBoxCollider.isTrigger)
            {
                m_Collider = ComponentBoxCollider;
                break;
            }
        }
    }

    void Update()
    {
        RayCast();

        if(m_PickedUp && Input.GetMouseButton(1))
        {
            float MouseX = Input.GetAxis("Mouse X") * (m_MouseSensitivity * 70);
            float MouseY = Input.GetAxis("Mouse Y") * (m_MouseSensitivity * 70);

            transform.localEulerAngles += new Vector3(MouseY, -MouseX, 0) * Time.deltaTime;
        }
    }

    void RayCast()
    {

    }

    void OnTriggerStay(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null && Collide.GetComponent<S_PcComponentHolder>().G_Component == m_Component)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            PcComponentHolderScript.G_MeshRenderer.enabled = true;
        }
    }
}
