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

    [Range(0f, 360f)]
    public float yawOffsetMax;
    [Range(0f, 90f)]
    public float pitchOffsetMax;

    public GameObject centralPoint;
    private float centralPointYaw;
    private float centralPointPitch;

    private void Awake()
    {
        Vector3 centralPointEuler = centralPoint.transform.rotation.eulerAngles;
        centralPointPitch = centralPointEuler.x;
        centralPointYaw = centralPointEuler.y;
    }

    void Update()
    {
        CheckConstraints();
    }

    public void CheckConstraints()
    {
        Vector3 targetDir = (target.transform.position - transform.position).normalized;

        yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;

        if (Mathf.Abs(yaw - centralPointYaw) > yawOffsetMax)
        {
            if (yaw < 0)
                yaw = centralPointYaw - yawOffsetMax;
            else
                yaw = centralPointYaw + yawOffsetMax;
        }

        if (Mathf.Abs(pitch - centralPointPitch) > pitchOffsetMax)
        {
            if (pitch < 0)
                pitch = centralPointPitch - pitchOffsetMax;
            else
                pitch = centralPointPitch + pitchOffsetMax;
        }

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
