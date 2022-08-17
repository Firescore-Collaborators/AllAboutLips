using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using NaughtyAttributes;

public class MainCameraController : MonoBehaviour
{

    public static MainCameraController instance;
    public List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    private CinemachineVirtualCamera currentCamera;

    Color initColor;

    public CinemachineVirtualCamera CurrentCamera
    {
        get
        {
            return currentCamera;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        InitCamera();

    }
    private void Start()
    {
    }
    void InitCamera()
    {
        initColor = Camera.main.backgroundColor;
        cameras.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            // CinemachineVirtualCamera cam = transform.GetChild(i).GetComponent<CinemachineVirtualCamera>();
            // if(cam != null)
            // {
            //     cameras.Add(cam);
            // }
            AddCameraFromChild(transform.GetChild(i));
        }
    }
    public void SetCameraZero()
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            cameras[i].Priority = 0;
        }
    }

    public void SetCurrentCamera(string camera, float blendSpeed = 1)
    {
        SetCameraZero();
        //cameras.Find(x => x.Name == camera).Priority = 10;
        //currentCamera = transform.Find(camera).GetComponent<CinemachineVirtualCamera>();
        currentCamera = cameras.Find(x => x.Name == camera);
        SetBlendSpeed(blendSpeed);
        currentCamera.Priority = 15;

    }

    void SetBlendSpeed(float blendSpeed)
    {
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = blendSpeed;
    }

    void AddCameraFromChild(Transform parent)
    {
        if (parent.TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera vcam))
        {
            cameras.Add(vcam);
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).TryGetComponent<CinemachineVirtualCamera>(out CinemachineVirtualCamera cam))
            {
                cameras.Add(cam);
            }

        }
    }

    public void SetCameraSolidColor(Color color)
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = color;
    }

    public void ResetCameraColor()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.backgroundColor = initColor;
    }
}

//Cherry Pick
