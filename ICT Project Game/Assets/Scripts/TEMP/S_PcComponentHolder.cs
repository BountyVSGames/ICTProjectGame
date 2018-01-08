using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BoxCollider))]
public class S_PcComponentHolder : MonoBehaviour
{
    [Header("Component Selecting")]
    [Tooltip("Select the component you want to link with this trigger")]
    [SerializeField]
    private e_Components m_Component;

    [SerializeField]
    private S_PcComponent m_PcComponentScript;

    [Space(5)]
    [Header("Debug Information")]
    [SerializeField]
    private Material m_SelectedMaterial;

    private MeshRenderer m_MeshRenderer;

    public e_Components G_Component
    {
        get { return m_Component; }
    }
    public MeshRenderer G_MeshRenderer
    {
        get { return m_MeshRenderer; }
    }
    public S_PcComponent S_PcComponentScript
    {
        set { m_PcComponentScript = value; }
    }

    void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
       if(m_PcComponentScript == null || (m_PcComponentScript != null && m_MeshRenderer.enabled && !m_PcComponentScript.G_PickedUp))
       {
            m_MeshRenderer.enabled = false;
        }
    }
}
