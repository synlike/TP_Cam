using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    [Range(-180f, 180f)]
    public float roll;
    [Range(0f, 180f)]
    public float fov;
    private float yaw;
    private float pitch;

    public float distance;

    public GameObject target;

    public Rail rail;
    public Vector3 railPosition;
    public float speed;


    void Awake()
    {
        transform.position = rail.transform.GetChild(0).position;
    }
    
    void Update()
    {
        Vector3 targetDir = (target.transform.position - transform.position).normalized;

        yaw = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(targetDir.y) * Mathf.Rad2Deg;

        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0)
        {
            distance += speed * Time.deltaTime;
        }
        else if(horizontal < 0)
        {
            distance -= speed * Time.deltaTime;
        }
        
        if (distance < 0)
        {
            if(rail.isLoop)
                distance = rail.maxDistance;
            else
                distance = 0;
        }
        else if(distance > rail.maxDistance)
        {
            if (rail.isLoop)
                distance = 0;
            else
                distance = rail.maxDistance;
        }

        rail.GetPosition(distance);

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
