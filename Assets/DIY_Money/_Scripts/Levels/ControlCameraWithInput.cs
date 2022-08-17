using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCameraWithInput : MonoBehaviour
{
    public Transform cameraTransformRight;
    public Transform cameraTransformLeft;
    public Transform cameraTransformUp;
    public Transform cameraTransformDown;
    public Transform currentTransform;
    public Camera cam;
    Cinemachine.CinemachineBrain cinemachineBrain;
    public bool control;
    public bool input;
    public Vector3 horizontalValue;
    public Vector3 verticalValue;
    [Range(0,1)]
    public float horizontalPos;
    [Range(0,1)]

    public float horizontalRot;
    [Range(0,1)]
    public float verticalPos;
    [Range(0,1)]
    public float verticalRot;
    public bool hInverseRot;
    public bool hInversePos;
    public bool vInverseRot;
    public bool vInversePos;

    /*void Start()
    {
        cameraTransformRight = transform.Find("Right");
        cameraTransformLeft = transform.Find("Left");
        cameraTransformUp = transform.Find("Up");
        cameraTransformDown = transform.Find("Down");
    }*/
    void OnEnable()
    {
        cam = Camera.main;
        cinemachineBrain = cam.GetComponent<Cinemachine.CinemachineBrain>();
        Timer.Delay(2, () =>
        {
            cinemachineBrain.enabled = false;

        });
    }

    void OnDisable()
    {
        if (cinemachineBrain != null)
            cinemachineBrain.enabled = true;
    }


    void Update()
    {
        if (!control) return;

        horizontalValue = new Vector3(cameraTransformLeft.position.x, currentTransform.position.x, cameraTransformRight.position.x);
        verticalValue = new Vector3(cameraTransformUp.position.y, currentTransform.position.y, cameraTransformDown.position.y);

        if (input)
        {
            horizontalPos = Remap.remap(currentTransform.position.x, cameraTransformRight.position.x, cameraTransformLeft.position.x, hInversePos ? 1 : 0, hInversePos ? 0 : 1);
            verticalPos = Remap.remap(currentTransform.position.z, cameraTransformUp.position.z, cameraTransformDown.position.z, vInversePos ? 1 : 0, vInverseRot ? 0 : 1);
            horizontalRot = Remap.remap(currentTransform.position.x, cameraTransformLeft.position.x, cameraTransformRight.position.x, hInverseRot ? 1 : 0, hInverseRot ? 0 : 1);
            verticalRot = Remap.remap(currentTransform.position.z, cameraTransformUp.position.z, cameraTransformDown.position.z, vInverseRot ? 1 : 0, vInverseRot ? 0 : 1);
        }

        float camX = Mathf.Lerp(cameraTransformLeft.position.x, cameraTransformRight.position.x, horizontalPos);
        cam.transform.position = new Vector3(camX, cam.transform.position.y, cam.transform.position.z);
        //Quaternion rotationX = Quaternion.Lerp(cameraTransformLeft.rotation, cameraTransformRight.rotation, horizontal);
        //Quaternion rotationY = Quaternion.Lerp(cameraTransformUp.rotation, cameraTransformDown.rotation, vertical);
        float xLerp = Mathf.LerpAngle(cameraTransformLeft.rotation.eulerAngles.y, cameraTransformRight.rotation.eulerAngles.y, horizontalRot);
        float yLerp = Mathf.LerpAngle(cameraTransformUp.rotation.eulerAngles.x, cameraTransformDown.rotation.eulerAngles.x, verticalRot);
        //float rotationY = Mathf.Lerp(cameraTransformLeft.rotation.eulerAngles.x, cameraTransformRight.rotation.eulerAngles.x, vertical);
        //cam.transform.rotation = Quaternion.Euler(new Vector3(rotationX.eulerAngles.x, rotationY.eulerAngles.y, cam.transform.rotation.z));
        //cam.transform.rotation = Quaternion.Lerp(cameraTransformLeft.rotation, cameraTransformRight.rotation, horizontal);
        cam.transform.eulerAngles = new Vector3(yLerp, xLerp, cam.transform.eulerAngles.z);
    }

}
