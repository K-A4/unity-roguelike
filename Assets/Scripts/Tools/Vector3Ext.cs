using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Ext 
{
    public static bool Equals(ref this Vector3 lhs , Vector3 rhs, float eps = 0.00001f)
    {
        return (lhs - rhs).sqrMagnitude < eps * eps;
    }
}
