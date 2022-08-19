using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHorizontalLerp : MonoBehaviour
{
    public static MouseHorizontalLerp instance;
    [Range(0, 1)]
    public float lerpValue;

    Vector3 currentMousePos, leftSceenPos, rightSceenPos;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    
    void Start()
    {
        leftSceenPos = new Vector3(0,Screen.height/2, 0);
        rightSceenPos = new Vector3(Screen.width, Screen.height/2, 0);
    }

    void Update()
    {
        GetLerpValue();
    }

    void GetLerpValue()
    {
        currentMousePos = Input.mousePosition;
        lerpValue = Remap.remap(currentMousePos.x, leftSceenPos.x, rightSceenPos.x, 0, 1,true,true,true,true);
    }
}
