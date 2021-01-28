using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MathUtils;

public class Curve : MonoBehaviour
{
    public Vector3 A, B, C, D;
    
    void Start()
    {
    }
    
    void Update()
    { 
    }

    public Vector3 GetPosition(float t)
    {
        Vector3 pos = CubicBezier(A, B, C, D, t);
        return pos;
    }

    public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
    {
        Vector3 pos = CubicBezier(A, B, C, D, t);
        Vector3 posWorld = localToWorldMatrix.MultiplyPoint(pos);

        return posWorld;
    }

    public void DrawGizmos(Color c, Matrix4x4 localToWorldMatrix)
    {
        
    }

}
