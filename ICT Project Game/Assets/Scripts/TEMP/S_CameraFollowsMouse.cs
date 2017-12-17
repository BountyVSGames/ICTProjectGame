using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(S_CameraFollowsMouse))]

public class S_CameraFollowsMouse : MonoBehaviour
{
    //Movement Variable's
    private float m_MoveFoward;
    private float m_TurnAround;
    private float m_RotateLeftRight;
    private float m_RotateUpDown;
    private float m_RotateY;
    [SerializeField]
    private float m_MouseSensitivity;
    private float m_CameraRange;
    private float m_VerticalRotation;
    [SerializeField]
    private float m_Speed;
    private Vector3 m_Move;

    private S_GameManager m_GameManagerScript;

    //Other Variables
    private bool m_ShowMouse;
    private GameObject m_HoldingComponent;
    private Vector3 m_PositionHolder;

    //Serialized Variable's
    [Header("Developer stuff. Do not touch")]
    [SerializeField]
    private float m_RayCastHeight;
    [SerializeField]
    private GameObject m_ComponentHolder;

    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_ShowMouse = false;

        m_CameraRange = 50.0f;
        m_VerticalRotation = 0;
        m_Speed = 4f;

        m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;
    }

    void Update()
    {
        //Get the rotation of the mouse and controller
        if(!Input.GetMouseButton(1))
        {
            Rotation();
        }
        //Get the movement using keyboard and controller
        Movement();
        //Create the raycast and look for a hit on a door
        if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
        {
            ComponentHolding();
        }

        transform.position += m_Move * m_Speed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            m_ShowMouse = !m_ShowMouse;
        }
        Cursor.visible = m_ShowMouse;
    }

    //Movement Voids
    void Movement()
    {
        //Instantiating Variable's
        m_MoveFoward = Input.GetAxis("Vertical");
        m_TurnAround = Input.GetAxis("Horizontal");

        //Adds The Movement
        m_Move = new Vector3(m_TurnAround, 0, m_MoveFoward);
        m_Move = transform.rotation * m_Move;
    }
    void Rotation()
    {
        //Adds the Mouse X speed to rotation for horizontal rotation
        m_RotateLeftRight = Input.GetAxis("Mouse X") * m_MouseSensitivity;
        transform.Rotate(0, m_RotateLeftRight, 0);
        //Adds the Mouse X speed to rotation for vertical rotation
        m_VerticalRotation -= Input.GetAxis("Mouse Y") * m_MouseSensitivity;
        //Keeps the rotation in a certain range
        m_VerticalRotation = Mathf.Clamp(m_VerticalRotation, -m_CameraRange, m_CameraRange);
        Camera.main.transform.localRotation = Quaternion.Euler(m_VerticalRotation, 0, 0);
    }

    void ComponentHolding()
    {
        if(m_HoldingComponent == null)
        {
                //Initializing Variable's
                RaycastHit hit;
                Vector3 RayCastStartPosition = new Vector3(transform.position.x, transform.position.y + m_RayCastHeight, transform.position.z);

                //Draw The Ray For Debugging
                Debug.DrawRay(RayCastStartPosition, Camera.main.transform.forward * 4.5f, Color.red);
                //RayCast Hit
                if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f) && hit.collider.GetComponent<S_PcComponent>() != null)
                {
                    GameObject Component = hit.collider.gameObject;
                    S_PcComponent ComponentScript = Component.GetComponent<S_PcComponent>();

                    Component.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                    for (int i = 0; i < Component.GetComponents<BoxCollider>().Length; i++)
                    {
                        BoxCollider ComponentBoxCollider = Component.GetComponents<BoxCollider>()[i];

                        if (!ComponentBoxCollider.isTrigger)
                        {
                            ComponentBoxCollider.enabled = false;
                            break;
                        }
                    }

                    m_PositionHolder = m_ComponentHolder.transform.localPosition;
                    m_ComponentHolder.transform.localPosition += new Vector3(0, 0, ((Component.transform.localScale.x + Component.transform.localScale.y) / 2));

                    ComponentScript.GS_PickedUp = true;

                    Component.transform.parent = m_ComponentHolder.transform;

                    Component.transform.localPosition = Vector3.zero;
                    Component.transform.localEulerAngles = Vector3.zero;

                    m_HoldingComponent = Component;
                    m_CameraRange = 30f;
            }
        }
        else if(m_HoldingComponent != null)
        {
            S_PcComponent ComponentScript = m_HoldingComponent.GetComponent<S_PcComponent>();
            ComponentScript.GS_PickedUp = false;

            m_HoldingComponent.transform.parent = null;

            m_HoldingComponent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            for (int i = 0; i < m_HoldingComponent.GetComponents<BoxCollider>().Length; i++)
            {
                BoxCollider ComponentBoxCollider = m_HoldingComponent.GetComponents<BoxCollider>()[i];

                if (!ComponentBoxCollider.isTrigger)
                {
                    ComponentBoxCollider.enabled = false;
                    break;
                }
            }

            m_HoldingComponent.GetComponent<BoxCollider>().enabled = true;

            //m_HoldingComponent.transform.localPosition = m_PositionHolder;

            m_HoldingComponent = null;
            m_CameraRange = 50f;
        }
    }
}