using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ICTProjectGame.Player
{
    public class S_Player : MonoBehaviour
    {
        [Header("Movement Variable's: ")]
        [SerializeField] private Vector3 m_Movement;
        [SerializeField] private Vector3 m_Rotation;
        [SerializeField] [Range(2, 6)] private float m_Speed = 2;
        [SerializeField] private float m_MouseSensitivity;

        private float m_PickUpDelay;

        [Space(5)]
        [Header("Object Selecting Info")]
        [SerializeField] private float m_RayLenght = 2.5f;
        [SerializeField] private UnityEngine.UI.Image m_CrosshairImage;
        [SerializeField] private Sprite[] m_CrosshairSprites;
        [SerializeField] private Material m_SelectedMaterial;
        [SerializeField] private Material m_SelectedOldMaterial;

        [Space(5)]
        [Header("Component Information")]
        private float m_Scrolling;

        private GameObject m_Camera;
        private GameObject m_PreviousSelectedObject;
        private GameObject m_LinkObject;

        private ICTProjectGame.Managment.S_GameManager m_GameManagerScript;

        private void Start()
        {
            m_Camera = Camera.main.gameObject;
            m_LinkObject = m_Camera.transform.GetChild(0).gameObject;

            m_CrosshairImage = GameObject.FindObjectOfType<UnityEngine.UI.Image>();

            m_GameManagerScript = GameObject.FindObjectOfType<ICTProjectGame.Managment.S_GameManager>();
            m_MouseSensitivity = m_GameManagerScript.G_MouseSensitivity;
        }

        private void Update()
        {
            Movement();

            if(!(Input.GetMouseButton(1) && m_LinkObject.transform.childCount > 0))
            {
                Rotation();
            }
            else
            {
                ComponentRotation();
            }

            if (m_LinkObject.transform.childCount < 1 && m_PickUpDelay <= 0)
            {
                Raycast();
            }
            else if(m_CrosshairImage.sprite != m_CrosshairSprites[0])
            {
                m_CrosshairImage.sprite = m_CrosshairSprites[0];
            }
            else
            {
                ComponentScrolling();
            }

            transform.position += m_Movement * m_Speed * Time.deltaTime;
        }

        #region Player Movement
            private void Movement()
            {
                float X = Input.GetAxis("Horizontal");
                float Z = Input.GetAxis("Vertical");

                m_Movement = transform.rotation * new Vector3(X, 0, Z);
            }
            private void Rotation()
            {
                float X = Input.GetAxis("Mouse X");
                float Y = -Input.GetAxis("Mouse Y");

                m_Camera.transform.eulerAngles += new Vector3(Y, 0, 0);
                transform.eulerAngles += new Vector3(0, X, 0);
            }
        #endregion

        #region Interaction Functions
            private void Raycast()
            {
                Ray RayCast = new Ray(m_Camera.transform.position, m_Camera.transform.forward);
                RaycastHit RayCastHit;

                Debug.DrawRay(RayCast.origin, RayCast.direction * m_RayLenght, Color.red);

                if (Physics.Raycast(RayCast, out RayCastHit, m_RayLenght) && RayCastHit.collider.tag == "Interactable")
                {
                    GameObject SelectedObject = RayCastHit.collider.gameObject;

                    if (Input.GetMouseButtonDown(0) && RayCastHit.collider.GetComponent<S_Interactable>())
                    {
                        S_Interactable SelectedObjectScript = RayCastHit.collider.GetComponent<S_Interactable>();

                        SelectedObjectScript.Connect(m_LinkObject, this);
                    }

                    m_CrosshairImage.sprite = m_CrosshairSprites[1];

                    m_PreviousSelectedObject = SelectedObject;
                }
                else if (m_PreviousSelectedObject != null)
                {
                    m_CrosshairImage.sprite = m_CrosshairSprites[0];
                }
            }
            public void Disconnect()
            {
                m_PickUpDelay = 1;
                StartCoroutine(DelayTimer());
            }

            private IEnumerator DelayTimer()
            {
                while(m_PickUpDelay > 0)
                {
                    m_PickUpDelay -= 0.25f;
                    yield return new WaitForSeconds(.1f);
                }
            }
        #endregion
        #region Component Functions
            private void ComponentScrolling()
            {
                m_Scrolling += Input.GetAxis("Mouse ScrollWheel");

                m_Scrolling = Mathf.Clamp(m_Scrolling, 1f, 2f);

                m_LinkObject.transform.localPosition = Vector3.Lerp(m_LinkObject.transform.localPosition, new Vector3(0, 0, m_Scrolling), .5f);
            }
            private void ComponentRotation()
            {
                float MouseX = Input.GetAxis("Mouse X") * (m_MouseSensitivity * 70);
                float MouseY = Input.GetAxis("Mouse Y") * (m_MouseSensitivity * 70);

            m_LinkObject.transform.localEulerAngles += transform.localRotation * new Vector3(MouseY, -MouseX, 0) * Time.deltaTime;
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, GetComponent<BoxCollider>().size);
        }
    }
}
