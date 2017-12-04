using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CameraFollowsMouse))]

public class CameraFollowsMouse : MonoBehaviour
{
    //UI Variable's
    private GameObject m_StanimaText;
    private GameObject m_InteractableDoorText;
    private bool m_InteractWithDoor;

    //Movement Variable's
    private float m_MoveFoward;
    private float m_TurnAround;
    private float m_RotateLeftRight;
    private float m_RotateUpDown;
    private float m_RotateY;
    private float m_MouseSensitivity = 5f;
    private float m_CameraRange = 60.0f;
    private float m_VerticalRotation = 0f;
    private float m_Speed;
    private float m_StanimaCounter = 100f;
    private Vector3 m_Move;

    //Other Variables
    private bool m_ShowMouse;

    //Serialized Variable's
    [SerializeField]
    private float m_RayCastHeight;

    void Start()
    {
        m_InteractWithDoor = false;
        m_ShowMouse = false;
    }

    void Update()
    {
        //Get the rotation of the mouse and controller
        Rotation();
        //Get the movement using keyboard and controller
        Movement();
        //Create the raycast and look for a hit on a door
        RayCast();

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

        //Checking if he can run. If is is true he will add speed and decrease the stanima, else he resets the speed and add more to stanima
        Stanima();

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
    void Stanima()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !(m_StanimaCounter <= 0))
        {
            m_Speed = 10f;
            m_StanimaCounter -= 0.25f;
        }
        else if (!(Input.GetKey(KeyCode.LeftShift)) && (m_StanimaCounter < 100))
        {
            m_Speed = 4f;
            m_StanimaCounter += 0.25f;
        }
        else
        {
            m_Speed = 4f;
        }
    }

    void RayCast()
    {
        //Initializing Variable's
        RaycastHit hit;
        Vector3 RayCastStartPosition = new Vector3(transform.position.x, transform.position.y + m_RayCastHeight, transform.position.z);

        //Draw The Ray For Debugging
        Debug.DrawRay(RayCastStartPosition, Camera.main.transform.forward * 4.5f, Color.red);
        //RayCast Hit
        if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f))
        {
            switch (hit.collider.tag)
            {

            }
        }
    }
}