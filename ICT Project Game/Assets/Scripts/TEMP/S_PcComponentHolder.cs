using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcComponentHolder : MonoBehaviour
{
    [Header("Component Selecting")]
    [Tooltip("Select the component you want to link with this trigger")]
    [SerializeField]
    private e_Components m_Component;

    [SerializeField]
    private S_PcComponent m_PcComponentScript;

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

        m_MeshRenderer.enabled = false;
    }

    void Update()
    {
       if(m_MeshRenderer.enabled && !m_PcComponentScript.GS_PickedUp)
       {
            m_MeshRenderer.enabled = false;
       }
    }
}
