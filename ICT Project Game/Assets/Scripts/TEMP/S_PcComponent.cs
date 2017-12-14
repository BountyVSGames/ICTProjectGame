using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcComponent : MonoBehaviour
{
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
            Debug.Log("FF");

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
        if(Collide.gameObject != this.gameObject && Collide.name == (this.name + "_Holder"))
        {
            Debug.Log("TEST1234");

        }
    }
}
