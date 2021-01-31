using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private static ViewVolumeBlender _instance;
    public static ViewVolumeBlender Instance { get { return _instance; } }

    private List<AViewVolume> activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> volumesPerViews = new Dictionary<AView, List<AViewVolume>>();

    private int maxPriority = 0;


    void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        foreach(AViewVolume volume in activeViewVolumes)
        {
            if(volume.priority > maxPriority)
            {
                maxPriority = volume.priority;
            }
        }
        foreach(AViewVolume volume in activeViewVolumes)
        {
            if(volume.priority <  maxPriority)
            {
                volume.view.weight = 0;
            }
            else
            {
                volume.view.weight = Mathf.Max(volume.view.weight, volume.ComputeSelfWeight());
            }
        }
    }

    public void AddVolume(AViewVolume viewVolume)
    {
        activeViewVolumes.Add(viewVolume);

        AView view = viewVolume.view;

        if (!volumesPerViews.ContainsKey(view))
        {
            List<AViewVolume> newList = new List<AViewVolume>();
            volumesPerViews.Add(view, newList);
            view.SetActive(true);
            volumesPerViews[view].Add(viewVolume);
        }
    }

    public void RemoveVolume(AViewVolume viewVolume)
    {
        AView view = viewVolume.view;

        activeViewVolumes.Remove(viewVolume);
        volumesPerViews[view].Remove(viewVolume);

        if(volumesPerViews[view].Count == 0)
        {
            volumesPerViews.Remove(view);
            view.SetActive(false);
        }
    }

    private void OnGUI()
    {
        int posY = 10;
        GUI.Label(new Rect(10, posY, 500, 500), "View Actives :");


        foreach(AViewVolume volume in activeViewVolumes)
        {
            posY += 20;
            GUI.Label(new Rect(10, posY, 500, 500), volume.gameObject.name);
        }

    }
}
