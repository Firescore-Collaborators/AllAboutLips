using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class LerpObjectLocalPositionBehavior : MonoBehaviour
{
    bool toLerp = false;
    float lerpSpeed;
    float lerpTime;
    
    Vector3 initPos;
    Vector3 finalPos;
    Transform currentObject;

    System.Action lerpComplete;

    
    void Update()
    {
        if(toLerp == false)
            return;

        currentObject.transform.localPosition = Vector3.Lerp(initPos,finalPos,lerpTime);

        if(lerpTime < 1.0f)
        {
            lerpTime += Time.deltaTime/lerpSpeed;
        }
        else{
            toLerp = false;
            lerpTime = 0;
            if(lerpComplete != null)
            {
                lerpComplete.Invoke();
            }
        }
        
    }
    public void LerpObject(Transform lerpObject, Vector3 _finalPos, float speed, System.Action _lerpComplete = null)
    {
        currentObject = lerpObject;
        initPos = currentObject.transform.localPosition;
        finalPos = _finalPos;
        lerpSpeed = speed;
        lerpTime = 0;
        if(_lerpComplete != null)
            lerpComplete = _lerpComplete;
        else
            lerpComplete = null;

        toLerp = true;
    }
}
