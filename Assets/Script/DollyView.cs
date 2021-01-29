using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyView : AView
{
    public bool isAuto;
    public float speedAutoDolly = 1;

    [Range(-180f, 180f)]
    public float roll;
    [Range(0f, 180f)]
    public float fov;
    private float yaw;
    private float pitch;

    public float distance;

    public GameObject target;

    //test
    public float currentDistance;

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

        if(isAuto)
        {
            AutoMove();
        }
        else
        {
            ManualMove();
        }
        UpdateCurrentDistance();
    }

    public void AutoMove()
    {
        int closestNode = FindClosestNode();
        Vector3 posPoint = new Vector3(0, 0, 0);
        float lerpDist = 0;

        if (closestNode == 0)
        {
            Vector3 vec1 = rail.transform.GetChild(closestNode + 1).position - rail.transform.GetChild(closestNode).position;
            Vector3 vecToTarget = target.transform.position - rail.transform.GetChild(closestNode).position;

            float projectedDistance = Vector3.Dot(vec1, vecToTarget) / vec1.magnitude;

            projectedDistance = Mathf.Clamp(projectedDistance, 0, vec1.magnitude);

            
            float actualDistance = GetActualDistance(closestNode, projectedDistance);
            
            lerpDist = Mathf.Lerp(currentDistance, actualDistance, Time.deltaTime);

            posPoint = rail.GetPosition(lerpDist, true);
        } 
        else if (closestNode == rail.GetLength() - 1)
        {
            Vector3 vec1 = rail.transform.GetChild(closestNode - 1).position - rail.transform.GetChild(closestNode).position;
            Vector3 vecToTarget = target.transform.position - rail.transform.GetChild(closestNode).position;

            float projectedDistance = Vector3.Dot(vec1, vecToTarget) / vec1.magnitude;

            projectedDistance = Mathf.Clamp(projectedDistance, 0, vec1.magnitude);

            float actualDistance = GetActualDistance(closestNode, projectedDistance);


            lerpDist = Mathf.Lerp(currentDistance, actualDistance, Time.deltaTime);

            posPoint = rail.GetPosition(lerpDist, true);
        }
        else
        {
            // Calcul sur segment gauche

            Vector3 vecLeft = rail.transform.GetChild(closestNode).position - rail.transform.GetChild(closestNode - 1).position;
            Vector3 vecToTargetLeft = target.transform.position - rail.transform.GetChild(closestNode - 1).position;

            float projectedDistanceLeft = Vector3.Dot(vecLeft, vecToTargetLeft) / vecLeft.magnitude;
            projectedDistanceLeft = Mathf.Clamp(projectedDistanceLeft, 0, vecLeft.magnitude);
            float actualDistanceLeft = GetActualDistance(closestNode - 1, projectedDistanceLeft);
            Vector3 posPointLeft = rail.GetPosition(actualDistanceLeft, false);


            // Calcul sur segment gauche
            Vector3 vecRight = rail.transform.GetChild(closestNode + 1).position - rail.transform.GetChild(closestNode).position;
            Vector3 vecToTargetRight = target.transform.position - rail.transform.GetChild(closestNode).position;

            float projectedDistanceRight = Vector3.Dot(vecRight, vecToTargetRight) / vecRight.magnitude;
            projectedDistanceRight = Mathf.Clamp(projectedDistanceRight, 0, vecRight.magnitude);
            float actualDistanceRight = GetActualDistance(closestNode, projectedDistanceRight);
            Vector3 posPointRight = rail.GetPosition(actualDistanceRight, false);


            // Distance la plus faible
            if(Vector3.Distance(posPointRight, target.transform.position) < Vector3.Distance(posPointLeft, target.transform.position))
            {
                distance = actualDistanceRight;
            }
            else
            {
                distance = actualDistanceLeft;
            }

            lerpDist = Mathf.Lerp(currentDistance, distance, speedAutoDolly * Time.deltaTime);

            posPoint = rail.GetPosition(lerpDist, true);
        }

        transform.position = posPoint;
    }

    public void ManualMove()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal > 0)
        {
            distance += speed * Time.deltaTime;
        }
        else if (horizontal < 0)
        {
            distance -= speed * Time.deltaTime;
        }

        CheckDistance();


        transform.position = rail.GetPosition(distance, false);
    }

    public void CheckDistance()
    {
        if (distance < 0)
        {
            if (rail.isLoop)
                distance = rail.maxDistance;
            else
                distance = 0;
        }
        else if (distance > rail.maxDistance)
        {
            if (rail.isLoop)
                distance = 0;
            else
                distance = rail.maxDistance;
        }
    }

    public int FindClosestNode()
    {
        int closestNode = 0;
        float closestDist = 0;

        for(int i = 0; i < rail.GetLength() - 1; i++)
        {
            float dist = Vector3.Distance(target.transform.position, rail.transform.GetChild(i).position);

            if (i == 0)
                closestDist = dist;
            else
            {
                if(dist < closestDist)
                {
                    closestDist = dist;
                    closestNode = i;
                }
            }
        }
        return closestNode;
    }

    public float GetActualDistance(int node, float distance)
    {
        float actualDist = 0;
        for (int i = 0; i < node; i++)
        {
            actualDist += Vector3.Distance(rail.transform.GetChild(i).position, rail.transform.GetChild(i + 1).position);
        }

        return actualDist + distance;
    }

    public void UpdateCurrentDistance()
    {
        int currentNode = rail.currentNode;

        Vector3 vec = rail.transform.GetChild(currentNode + 1).position - rail.transform.GetChild(currentNode).position;
        Vector3 vecToSelf = transform.position - rail.transform.GetChild(currentNode).position;

        float projectedDistance = Vector3.Dot(vec, vecToSelf) / vec.magnitude;
        projectedDistance = Mathf.Clamp(projectedDistance, 0, vec.magnitude);

        currentDistance = GetActualDistance(currentNode, projectedDistance);

        Debug.Log(currentDistance);
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
