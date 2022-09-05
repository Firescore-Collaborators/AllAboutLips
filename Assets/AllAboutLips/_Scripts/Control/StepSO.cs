using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public enum StepType
{
    clean = 0,
    painting = 1,
    stencil = 2,
    sucker = 3,
    levelEnd,

}
public class StepSO : ScriptableObject
{

    public StepType stepType;

    //[Dropdown("CameraValue")]
    public string cameraName;
    public int blendSpeed;

    //private List<string> CameraValue { get { return new List<string>() { "Default", "TopDown","Empty","Sticker"}; } }

    public virtual void OnStepStart()
    {
        if (cameraName.Equals("Empty"))
        {
            return;
        }
        Timer.Delay(0.1f, () =>
        {
            MainCameraController.instance.SetCurrentCamera(cameraName, blendSpeed);
        });
    }

    public virtual void OnStepEnd()
    {
        Debug.Log("OnStepEnd");
    }
}
