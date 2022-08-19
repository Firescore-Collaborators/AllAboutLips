using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotateInterpolate : MonoBehaviour
{
    [SerializeField] Transform leftRotation;
    [SerializeField] Transform rightRotation;


    void Update()
    {
        transform.rotation = Quaternion.Lerp(leftRotation.rotation, rightRotation.rotation, MouseHorizontalLerp.instance.lerpValue);
    }
}
