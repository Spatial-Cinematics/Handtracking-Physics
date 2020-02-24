using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FloatExtensions
{
    private static bool CloserTo(this float value, float to, float than) {

        return Mathf.Abs(value) + Mathf.Abs(to) < Mathf.Abs(value) + Mathf.Abs(than);

    }
    
    public static float Remap (this float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
}
