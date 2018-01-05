﻿using System.Collections;
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
    private bool m_PcComponentHolderActive;

    private float m_MouseSensitivity;

    public bool GS_PickedUp
    {
        get { return m_PickedUp; }
        set { m_PickedUp = value; }
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

    public void Connect(GameObject GameObjectToPlugWith)
    {

    }

    public void Disconnect()
    {
        
    }

    void OnTriggerStay(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null && Collide.GetComponent<S_PcComponentHolder>().G_Component == m_Component)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            m_PcComponentHolderActive = true;

            if (!PcComponentHolderScript.G_MeshRenderer[0].enabled)
            {
                for (int i = 0; i < PcComponentHolderScript.G_MeshRenderer.Count; i++)
                {
                    PcComponentHolderScript.G_MeshRenderer[i].enabled = true;
                }

                PcComponentHolderScript.GetComponent<MeshFilter>().mesh = GetComponent<MeshFilter>().mesh;

                PcComponentHolderScript.transform.localScale = transform.localScale;

                PcComponentHolderScript.S_PcComponentScript = this;
            }

            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {

            }
        }
    }

    void OnTriggerExit(Collider Collide)
    {
        if(Collide.gameObject != this.gameObject && Collide.GetComponent<S_PcComponentHolder>() != null)
        {
            S_PcComponentHolder PcComponentHolderScript = Collide.GetComponent<S_PcComponentHolder>();

            for (int i = 0; i < PcComponentHolderScript.G_MeshRenderer.Count; i++)
            {
                PcComponentHolderScript.G_MeshRenderer[i].enabled = false;
            }

            m_PcComponentHolderActive = false;
        }
    }
}
