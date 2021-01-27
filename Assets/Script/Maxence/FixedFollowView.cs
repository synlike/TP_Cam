using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FixedFollowView : AView
{
    [Range(-180f, 180f)]
    public float roll;

    [Range(0f, 180f)]
    public float fov;

    public GameObject target;

    private float yaw;
    private float pitch;

    private float yawOffsetMax;
    private float pitchOffsetMax;
    private GameObject centralPoint;


    void Awake()
    {
        Vector3 targetDir = (target.transform.position - transform.position).normalized;

        yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;
    }

    public override CameraConfiguration GetConfiguration()
    {
        CameraConfiguration configuration = new CameraConfiguration();

        configuration.pitch = pitch;
        configuration.yaw = yaw;
        configuration.roll = roll;
        configuration.fieldOfView = fov;
        configuration.distance = 0;
        configuration.pivot = transform.position;

        return configuration;
    }
}
