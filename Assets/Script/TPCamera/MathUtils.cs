﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3 LinearBezier(Vector3 A, Vector3 B, float t)
    {
        Vector3 linearBezier = (1 - t) * A + t * B;
        return linearBezier;
    }

    public static Vector3 QuadraticBezier(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        Vector3 quadraticBezier = (1 - t) * LinearBezier(A, B, t) + t * LinearBezier(B, C, t);
        return quadraticBezier;
    }

    public static Vector3 CubicBezier(Vector3 A, Vector3 B, Vector3 C, Vector3 D, float t)
    {
        Vector3 cubicBezier = (1 - t) * QuadraticBezier(A, B, C, t) + t * QuadraticBezier(B, C, D, t);
        return cubicBezier;
    }
}
