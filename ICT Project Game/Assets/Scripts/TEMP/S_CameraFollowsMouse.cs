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
    private float m_CameraRange;
    private float m_VerticalRotation;

    //Serialized Movement Variable's
    private Vector3 m_Move;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_MouseSensitivity;
    private float m_Scrolling;

    //Game Manager Script
    private S_GameManager m_GameManagerScript;

    //Other Variables
    private GameObject m_HoldingComponent;
    private S_PcComponent m_PcComponentScript;
    private Vector3 m_PositionHolder;

    //Serialized Variable's
    [Space(5)]
    [Header("Debug Information:")]
    [SerializeField]
    private float m_RayCastHeight;
    [SerializeField]
    private GameObject m_ComponentHolder;

    //Initializing Variable's
    void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_CameraRange = 50.0f;
        m_VerticalRotation = 0;
        m_Speed = 4f;
        m_Scrolling = 0;

        m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;
    }

    //Update, runs every frame
    void Update()
    {
        //Get the rotation of the mouse and controller
        if(!Input.GetMouseButton(1))
        {
            Rotation();
        }
        else
        {
            ComponentRotation();
        }
        //Get the movement using keyboard and controller
        Movement();
        //Create the raycast and look for a hit on a door
        if((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !(m_PcComponentScript != null && m_PcComponentScript.G_PcComponentHolderActive))
        {
            ComponentHolding();
        }
        //Allowing Scrolling if the component is attached to the 'arm'
        if(m_HoldingComponent != null)
        {
            ComponentScrolling();
        }
        //Let's a move
        transform.position += m_Move * m_Speed * Time.deltaTime;
    }

    #region Player Movement
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
    #endregion
    #region Component Movement
    void ComponentScrolling()
        {
            m_Scrolling += Input.GetAxis("Mouse ScrollWheel");

            m_Scrolling = Mathf.Clamp(m_Scrolling, 1f, 2f);

            m_ComponentHolder.transform.localPosition = Vector3.Lerp(m_ComponentHolder.transform.localPosition, new Vector3(0, 0, m_Scrolling), .5f);
        }
        void ComponentRotation()
        {
            float MouseX = Input.GetAxis("Mouse X") * (m_MouseSensitivity * 70);
            float MouseY = Input.GetAxis("Mouse Y") * (m_MouseSensitivity * 70);

            m_ComponentHolder.transform.localEulerAngles += transform.localRotation * new Vector3(MouseY, -MouseX, 0) * Time.deltaTime;
        }
    #endregion

    #region Component Holding
    void ComponentHolding()
        {
            if (m_HoldingComponent == null)
            {
                //Initializing Variable's
                RaycastHit hit;
                Vector3 RayCastStartPosition = new Vector3(transform.position.x, transform.position.y + m_RayCastHeight, transform.position.z);

                //Draw The Ray For Debugging
                Debug.DrawRay(RayCastStartPosition, Camera.main.transform.forward * 4.5f, Color.red);
                //RayCast Hit
                if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f) && hit.collider.GetComponent<S_PcComponent>() != null)
                {
                    Connect(hit.collider.gameObject);
                }
            }
            else if (m_HoldingComponent != null)
            {
                Disconnect();
            }
        }

        public void Connect(GameObject Component)
        {
            S_PcComponent ComponentScript = Component.GetComponent<S_PcComponent>();

            m_PcComponentScript = ComponentScript;

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

            m_Scrolling = Component.transform.localPosition.z;

            Component.transform.localPosition = Vector3.zero;

            m_HoldingComponent = Component;
            m_CameraRange = 30f;
        }

        public void Disconnect()
        {
            S_PcComponent ComponentScript = m_HoldingComponent.GetComponent<S_PcComponent>();
            ComponentScript.GS_PickedUp = false;

            m_PcComponentScript = null;

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

            m_HoldingComponent = null;
            m_CameraRange = 50f;
        }
    #endregion
}