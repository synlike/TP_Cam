using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private int length;

    public bool isLoop;

    public float maxDistance;

    void Start()
    {
        length = transform.childCount;

        maxDistance = GetMaxDistance();

        Vector3 test = GetPosition(-15);
    }
    
    void Update()
    {
        
    }

    public int GetLength()
    {
        return length;
    }

    public Vector3 GetPosition(float distance)
    {
        Vector3 direction;

        Vector3 posPoint = new Vector3(0, 0, 0);

        float distGlobal = 0;

        for (int i = 0; i < length - 1; i++)
        {
            direction = transform.GetChild(i+1).position - transform.GetChild(i).position;
            distGlobal += direction.magnitude;


            if (distGlobal >= distance)
            {
                posPoint = transform.GetChild(i + 1).position + (direction.normalized * (distance - distGlobal));
                break;
            }
            else if(i == length - 2)
            {
                if (isLoop)
                {
                    distance -= distGlobal;
                    distGlobal = 0;
                    i = -1;
                }
                else
                {
                    posPoint = transform.GetChild(i + 1).position;
                    distance = distGlobal;
                }
            }
        }
        
        return posPoint;
    }

    public float GetMaxDistance()
    {
       float distance = 0;
       for (int i = 0; i < length - 1; i++)
       {
            Vector3 direction = transform.GetChild(i + 1).position - transform.GetChild(i).position;
            distance += direction.magnitude;
       }

        return distance;
    }

}
