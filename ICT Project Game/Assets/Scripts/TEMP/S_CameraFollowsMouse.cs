using UnityEngine;
using UnityEngine.UI;
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
    [Header("Movement Variable's:")]
    private Vector3 m_Move;
    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float m_MouseSensitivity;
    private float m_Scrolling;

    [Space(5)]
    [Header("Changeable Materials:")]
    [SerializeField]
    private Sprite[] m_CrosshairImages;
    private Image m_CrosshairImage;
    [SerializeField]
    private Material m_SelectedMaterial;
    [SerializeField]
    private Material m_OldMaterial;

    //Game Manager Script
    private S_GameManager m_GameManagerScript;

    //Other Variables
    private GameObject m_HoldingComponent;
    private GameObject m_PreviousSelectedObject;
    private S_PcComponent m_PcComponentScript;

    private Vector3 m_ControllerForce;

    //Serialized Variable's
    [Space(5)]
    [Header("Debug Information:")]
    [SerializeField]
    private float m_RayCastHeight;
    [SerializeField]
    private GameObject m_ComponentHolder;

    //Get Setters
    public S_PcComponent S_PcComponentScript
    {
        set { m_PcComponentScript = value; }
    }
    public GameObject S_HoldingComponent
    {
        set { m_HoldingComponent = value; }
    }
    public Vector3 G_ControllerForce
    {
        get { return m_ControllerForce; }
    }

    //Initializing Variable's
    private void Start()
    {
        m_GameManagerScript = GameObject.FindWithTag("GameController").GetComponent<S_GameManager>();

        m_CrosshairImage = GameObject.FindWithTag("Canvas").transform.GetChild(0).GetComponent<Image>();

        m_CameraRange = 60.0f;
        m_VerticalRotation = 0;
        m_Speed = 2f;
        m_Scrolling = 0;

        m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;

        StartCoroutine(CalculateForce());
    }

    //Update, runs every frame
    private void Update()
    {
        //All component holding handling
        if(m_GameManagerScript.G_GameState == 1)
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

            //Allowing Scrolling if the component is attached to the 'arm'
            if (m_HoldingComponent != null)
            {
                ComponentScrolling();
            }
        }
        else
        {
            Rotation();
        }

        //Create the raycast and look for a hit on a door
        ComponentHolding();

        //Get the movement using keyboard and controller
        Movement();
        //Let's a move
        transform.position += m_Move * m_Speed * Time.deltaTime;
    }

    #region Player Movement
        private void Movement()
        {
            //Instantiating Variable's
            m_MoveFoward = Input.GetAxis("Vertical");
            m_TurnAround = Input.GetAxis("Horizontal");

            //Adds The Movement
            m_Move = new Vector3(m_TurnAround, 0, m_MoveFoward);
            m_Move = transform.rotation * m_Move;
        }
        private void Rotation()
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
        private void ComponentScrolling()
        {
            m_Scrolling += Input.GetAxis("Mouse ScrollWheel");

            m_Scrolling = Mathf.Clamp(m_Scrolling, 1f, 2f);

            m_ComponentHolder.transform.localPosition = Vector3.Lerp(m_ComponentHolder.transform.localPosition, new Vector3(0, 0, m_Scrolling), .5f);
        }

        private void ComponentRotation()
        {
            float MouseX = Input.GetAxis("Mouse X") * (m_MouseSensitivity * 70);
            float MouseY = Input.GetAxis("Mouse Y") * (m_MouseSensitivity * 70);

            m_ComponentHolder.transform.localEulerAngles += transform.localRotation * new Vector3(MouseY, -MouseX, 0) * Time.deltaTime;
        }
    #endregion
    #region Component Holding
        private void ComponentHolding()
        {
            if (m_HoldingComponent == null && m_GameManagerScript.G_GameState == 1)
            {
                //Initializing Variable's
                RaycastHit hit;
                Vector3 RayCastStartPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

                //Draw The Ray For Debugging
                Debug.DrawRay(RayCastStartPosition, Camera.main.transform.forward * 4.5f, Color.red);
                //RayCast Hit
                if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f) && hit.collider.GetComponent<S_PcComponent>() != null)
                {
                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !(m_HoldingComponent != null && m_PcComponentScript.G_PcComponentHolderActive))
                    {
                        Connect(hit.collider.gameObject);
                    }

                    m_CrosshairImage.sprite = m_CrosshairImages[1];
                }
                else
                {
                    m_CrosshairImage.sprite = m_CrosshairImages[0];
                }
            }
            else if (m_HoldingComponent != null)
            {
                if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && !(m_HoldingComponent != null && m_PcComponentScript.G_PcComponentHolderActive))
                {
                    Disconnect();
                }
            }
            else if(m_GameManagerScript.G_GameState == 0)
            {
                //Initializing Variable's
                RaycastHit hit;
                Vector3 RayCastStartPosition = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);

                //Draw The Ray For Debugging
                Debug.DrawRay(RayCastStartPosition, Camera.main.transform.forward * 4.5f, Color.red);
                //RayCast Hit
                if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f) && hit.collider.tag == "Wristband")
                {
                    MeshRenderer CollidedMeshRenderer = hit.collider.GetComponent<MeshRenderer>();

                    if(m_PreviousSelectedObject != null)
                    {
                        m_PreviousSelectedObject.GetComponent<MeshRenderer>().materials[1] = null;
                    }

                    if(!(CollidedMeshRenderer.materials.Length > 1))
                    {
                        m_OldMaterial = CollidedMeshRenderer.materials[0];

                        CollidedMeshRenderer.materials = new Material[2];

                        CollidedMeshRenderer.materials[0] = m_OldMaterial;
                        CollidedMeshRenderer.materials[1] = m_SelectedMaterial;
                    }
                    else
                    {
                        CollidedMeshRenderer.materials[1] = m_SelectedMaterial;
                    }

                    if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)))
                    {
                        m_GameManagerScript.GoToScene("Game");
                    }

                    m_PreviousSelectedObject = hit.collider.gameObject;

                    m_CrosshairImage.sprite = m_CrosshairImages[1];
                }
                else if(m_PreviousSelectedObject != null)
                {
                    m_PreviousSelectedObject.GetComponent<MeshRenderer>().materials[1] = null;

                    m_CrosshairImage.sprite = m_CrosshairImages[0];
                }   
            }
        }

        public void Connect(GameObject Component)
        {
            S_PcComponent ComponentScript = Component.GetComponent<S_PcComponent>();
            m_PcComponentScript = ComponentScript;

            ComponentScript.Connect(this.gameObject, m_ComponentHolder, this);

            m_HoldingComponent = Component;

            m_CameraRange = 30f;
            m_Scrolling = Component.transform.localPosition.z;
    }

        public void Disconnect()
        {
        if(m_HoldingComponent != null)
        {
            S_PcComponent ComponentScript = m_HoldingComponent.GetComponent<S_PcComponent>();

            ComponentScript.Disconnect();

            m_PcComponentScript = null;
            m_HoldingComponent = null;
        }

            m_CameraRange = 60f;
        }
    #endregion

    private IEnumerator CalculateForce()
        {
            while (true)
            {
                Quaternion Position = Camera.main.transform.localRotation * transform.localRotation;

                //Debug.Log(Position);

                yield return S_WaitFor.Frames(8);
                
                Quaternion NewPosition = Camera.main.transform.localRotation * transform.localRotation;

                m_ControllerForce = new Vector3(Position.x - NewPosition.x, Position.y - NewPosition.y, Position.z - NewPosition.z);
            }
        }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}