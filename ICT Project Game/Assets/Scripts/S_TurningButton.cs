using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_TurningButton : MonoBehaviour
{
    private void Update()
    {
        transform.eulerAngles += new Vector3(0, 1f, 1f) * 100 * Time.deltaTime;
    }
}
