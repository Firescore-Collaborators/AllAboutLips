using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLerpCustom : MonoBehaviour
{
    [Range(0, 1)]
    public float lerpValue;

    Vector3 currentMousePos;
    public float pos1Value, pos2Value;
    public bool isHorizontal;

    void Update()
    {
        GetLerpValue();
    }

    void GetLerpValue()
    {
        currentMousePos = Input.mousePosition;
        lerpValue = Remap.remap(isHorizontal ? transform.position.x : transform.position.y, pos1Value, pos2Value, 0, 1, true, true, true, true);
        //Debug.Log(transform.position.y);
    }
}
