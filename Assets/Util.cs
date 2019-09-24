using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static float AngleBetweenVector2(Vector2 v1, Vector2 v2)
    {
        float sign = Mathf.Sign(v1.x * v2.y - v1.y * v2.x);
        return Vector2.Angle(v1, v2) * sign;
    }

}
