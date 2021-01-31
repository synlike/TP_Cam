using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    List<AView> activeViews = new List<AView>();

    float speed = 0.5f;

    public static CameraController instance;
    public CameraConfiguration cameraConfiguration;

    public CameraConfiguration configCourante;
    public CameraConfiguration configCible;

    private bool isCutRequested;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Smoothing();

        PlaceCamera(cameraConfiguration);

        cameraConfiguration = GetConfigurationMoyenne();

        if(isCutRequested)
        {
            configCourante = configCible;
            isCutRequested = false;
        }
    }

    public void AddView(AView view)
    {
        activeViews.Add(view);
    }

    public void RemoveView(AView view)
    {
        activeViews.Remove(view);
    }

    public void PlaceCamera(CameraConfiguration config)
    {
        transform.position = config.GetPosition();
        transform.rotation = config.GetRotation();

        Camera.main.fieldOfView = config.fieldOfView;
    }

    public void Smoothing()
    {

        if (speed * Time.deltaTime < 1)
        {
            configCourante.pitch = configCourante.pitch + (configCible.pitch - configCourante.pitch) * speed * Time.deltaTime;
            configCourante.yaw = configCourante.yaw + (configCible.yaw - configCourante.yaw) * speed * Time.deltaTime;
            configCourante.roll = configCourante.roll + (configCible.roll - configCourante.roll) * speed * Time.deltaTime;

            configCourante.fieldOfView = configCourante.fieldOfView + (configCible.fieldOfView - configCourante.fieldOfView) * speed * Time.deltaTime;

            configCourante.pivot = configCourante.pivot + (configCible.pivot - configCourante.pivot) * speed * Time.deltaTime;
            configCourante.distance = configCourante.distance + (configCible.distance - configCourante.distance) * speed * Time.deltaTime;
        }
        else configCourante = configCible;

        cameraConfiguration = configCourante;
    }

    public CameraConfiguration GetConfigurationMoyenne()
    {
        CameraConfiguration configMoyenne = new CameraConfiguration();

        float poidsTotal = 0;

        foreach(AView view in activeViews)
        {
            poidsTotal += view.weight;
            configMoyenne.pitch += view.GetConfiguration().pitch * view.weight;
            configMoyenne.yaw += Vector2.SignedAngle(Vector2.right, new Vector2(Mathf.Cos(view.GetConfiguration().yaw * Mathf.Deg2Rad), Mathf.Sin(view.GetConfiguration().yaw * Mathf.Deg2Rad))) * view.weight;
            configMoyenne.roll += view.GetConfiguration().roll * view.weight;
            configMoyenne.pivot += view.GetConfiguration().pivot * view.weight;
            configMoyenne.fieldOfView += view.GetConfiguration().fieldOfView * view.weight;
        }
        
        configMoyenne.pitch /= poidsTotal;
        configMoyenne.yaw /= poidsTotal;
        configMoyenne.roll /= poidsTotal;
        configMoyenne.pivot /= poidsTotal;
        configMoyenne.fieldOfView /= poidsTotal;

        return configMoyenne;
    }

    public void Cut()
    {

    }

    public void OnDrawGizmos()
    {
        //cameraConfiguration.DrawGizmos(Color.yellow);
        foreach(AView view in activeViews)
        {
            view.GetConfiguration().DrawGizmos(Color.blue);
        }
    }
}       
