using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExt
{
    public static float Approach(float a, float b, float c)
    {
        float result;

        if (a > b)
        {
            result = Mathf.Max(a - c, b);
        }
        else
        {
            result = Mathf.Min(a + c, b);
        }
        return result;
    }
}
