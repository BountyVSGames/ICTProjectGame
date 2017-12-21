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

    private List<MeshRenderer> m_MeshRenderers;

    public e_Components G_Component
    {
        get { return m_Component; }
    }
    public List<MeshRenderer> G_MeshRenderer
    {
        get { return m_MeshRenderers; }
    }
    public S_PcComponent S_PcComponentScript
    {
        set { m_PcComponentScript = value; }
    }



    void Awake()
    {
        m_MeshRenderers = new List<MeshRenderer>();
    }

    void Start()
    {
        m_MeshRenderers.Add(GetComponent<MeshRenderer>());

        for (int i = 0; i < transform.childCount; i++)
        {
            m_MeshRenderers.Add(transform.GetChild(i).GetComponent<MeshRenderer>());
        }
    }

    void Update()
    {
       if(m_PcComponentScript == null || (m_PcComponentScript != null && m_MeshRenderers[0].enabled && !m_PcComponentScript.GS_PickedUp))
       {
            for (int i = 0; i < m_MeshRenderers.Count; i++)
            {
                m_MeshRenderers[i].enabled = false;
            }
        }
    }
}
