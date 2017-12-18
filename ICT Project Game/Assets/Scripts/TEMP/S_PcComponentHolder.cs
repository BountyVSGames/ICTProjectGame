using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcComponentHolder : MonoBehaviour
{
    [Header("Component Selecting")]
    [Tooltip("Select the component you want to link with this trigger")]
    [SerializeField]
    private e_Components m_Component;

    private MeshRenderer m_MeshRenderer;

    public e_Components G_Component
    {
        get { return m_Component; }
    }
    public MeshRenderer G_MeshRenderer
    {
        get { return m_MeshRenderer; }
    }


    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();

        m_MeshRenderer.enabled = false;
    }
}
