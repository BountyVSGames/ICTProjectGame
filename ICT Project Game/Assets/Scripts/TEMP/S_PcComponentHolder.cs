using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcComponentHolder : MonoBehaviour
{
    [SerializeField]
    private e_Components m_Component;

    private MeshRenderer m_MeshRender;

    public e_Components G_Component
    {
        get { return m_Component; }
    }
    public MeshRenderer G_MeshRenderer
    {
        get { return m_MeshRender; }
    }


    void Start()
    {
        m_MeshRender = GetComponent<MeshRenderer>();

        m_MeshRender.enabled = false;
    }

    void Update()
    {

    }
}
