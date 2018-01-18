using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class S_WaitFor
{
    public static IEnumerator Frames(int FramesCount)
    {
        while (FramesCount > 0)
        {
            FramesCount--;
            yield return null;
        }
    }
}