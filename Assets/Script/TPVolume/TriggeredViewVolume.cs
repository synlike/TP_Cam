using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggeredViewVolume : AViewVolume
{
    [SerializeField]
    private GameObject target;

    void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(target.gameObject.tag))
        {
            Debug.Log("wesh");
            SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(target.gameObject.tag))
        {
            SetActive(false);
        }
    }
}
