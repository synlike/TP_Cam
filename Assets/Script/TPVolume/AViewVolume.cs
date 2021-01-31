using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AViewVolume : MonoBehaviour
{
    public int priority = 0;
    public AView view;

    public bool isCutOnSwitch;

    protected bool IsActive { get; private set; }
    
    void Start()
    {
    }


    void Update()
    {
    }

    public virtual float ComputeSelfWeight()
    {


        return 1.0f;
    }

    protected void SetActive(bool isActive)
    {
        if(isActive)
            ViewVolumeBlender.Instance.AddVolume(this);
        else
            ViewVolumeBlender.Instance.RemoveVolume(this);

        IsActive = isActive;

        if(isCutOnSwitch)
        {
            ViewVolumeBlender.Instance.Update();
            CameraController.instance.Cut();
        }
    }
}
