using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotateInterpolate : MonoBehaviour
{
    [SerializeField] Transform leftRotation;
    [SerializeField] Transform rightRotation;
    [SerializeField] MouseLerpCustom lerpCustom;

    void Update()
    {
        transform.rotation = Quaternion.Lerp(leftRotation.rotation, rightRotation.rotation, lerpCustom == null ? MouseHorizontalLerp.instance.lerpValue : lerpCustom.lerpValue);
    }
}
