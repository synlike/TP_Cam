using UnityEngine;

public abstract class AView : MonoBehaviour
{
    public float weight;
    public bool isActiveOnStart;

    private void Start()
    {
        if (isActiveOnStart)
        {
            SetActive(isActiveOnStart);
        }
    }

    public void SetActive(bool isActive)
    {
        if (isActive) CameraController.instance.AddView(this);
        else CameraController.instance.RemoveView(this);
    }

    public virtual CameraConfiguration GetConfiguration()
    {
        return null;
    }
}
