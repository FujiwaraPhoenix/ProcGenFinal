using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFxns
{
    public static Vector3 ToVect(float a)
    {
        return new Vector3(Mathf.Cos(a * Mathf.Deg2Rad), Mathf.Sin(a * Mathf.Deg2Rad), 0);
    }

    public static float ToAng(Vector3 a)
    {
        return Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
    }
}