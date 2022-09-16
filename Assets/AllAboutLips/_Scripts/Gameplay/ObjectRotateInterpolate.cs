using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectRotateInterpolate : MonoBehaviour
{
    [SerializeField] Transform leftRotation;
    [SerializeField] Transform rightRotation;
    [SerializeField] MouseLerpCustom lerpCustom;

    public bool held;

    void SetInput()
    {
        if(EventSystem.current.IsPointerOverGameObject()) {return;}
        if (Input.GetMouseButtonDown(0))
        {
            held = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            held = false;
        }
    }
    void Update()
    {
        SetInput();
        if(!held) {return;}
        transform.rotation = Quaternion.Lerp(leftRotation.rotation, rightRotation.rotation, lerpCustom == null ? MouseHorizontalLerp.instance.lerpValue : lerpCustom.lerpValue);
    }
}
