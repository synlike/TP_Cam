using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereViewVolume : AViewVolume
{
    public GameObject target;
    public float outerRadius;
    public float innerRadius;

    private float distance;



    void Start()
    {
        
    }


    void Update()
    {
        distance = Vector3.Distance(target.transform.position, transform.position);

        if(distance <= outerRadius && !IsActive)
        {
            SetActive(true);
        }
        else if(distance > outerRadius && IsActive)
        {
            SetActive(false);
        }
    }

    public override float ComputeSelfWeight()
    {
        // ¯\_(ツ)_/¯

        return 1f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
    }
}
