﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(CameraFollowsMouse))]

public class CameraFollowsMouse : MonoBehaviour
{
    //Movement Variable's
    private float m_MoveFoward;
    private float m_TurnAround;
    private float m_RotateLeftRight;
    private float m_RotateUpDown;
    private float m_RotateY;
    private float m_MouseSensitivity;
    private float m_CameraRange;
    private float m_VerticalRotation;
    [SerializeField]
    private float m_Speed;
    private Vector3 m_Move;

    //Other Variables
    private bool m_ShowMouse;
    private GameObject m_HoldingComponent;

    //Serialized Variable's
    [Header("Developer stuff. Do not touch")]
    [SerializeField]
    private float m_RayCastHeight;
    [SerializeField]
    private GameObject m_ComponentHolder;

    void Start()
    {
        m_ShowMouse = false;

        m_CameraRange = 30.0f;
        m_MouseSensitivity = 2f;
        m_VerticalRotation = 0;
        m_Speed = 4f;
    }

    void Update()
    {
        //Get the rotation of the mouse and controller
        Rotation();
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
            if (Physics.Raycast(RayCastStartPosition, Camera.main.transform.forward, out hit, 4.5f))
            {
                switch (hit.collider.tag)
                {
                    case "Pickable":
                        GameObject Component = hit.collider.gameObject;
                        PcComponent ComponentScript = Component.GetComponent<PcComponent>();

                        Component.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                        Component.GetComponent<BoxCollider>().enabled = false;

                        ComponentScript.GS_PickedUp = true;

                        Component.transform.parent = m_ComponentHolder.transform;

                        Component.transform.localPosition = Vector3.zero;
                        Component.transform.localEulerAngles = Vector3.zero;

                        m_HoldingComponent = Component;
                        break;
                }
            }
        }
        else if(m_HoldingComponent != null)
        {
            PcComponent ComponentScript = m_HoldingComponent.GetComponent<PcComponent>();
            ComponentScript.GS_PickedUp = false;

            m_HoldingComponent.transform.parent = null;

            m_HoldingComponent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            m_HoldingComponent.GetComponent<BoxCollider>().enabled = true;

            m_HoldingComponent = null;
        }
    }
}