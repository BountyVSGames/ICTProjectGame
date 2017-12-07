using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PcComponent : MonoBehaviour
{
    private bool m_PickedUp;

    public bool GS_PickedUp
    {
        get { return m_PickedUp; }
        set { m_PickedUp = value; }
    }
}
