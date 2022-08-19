using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFloatValue : MonoBehaviour
{
    public static LerpFloatValue instance;
    bool toLerp;
    float lerpSpeed;
    float lerpTime;

    Vector3 initPos1, initPos2;
    float finalValue;
    float startValue;

    System.Action lerpComplete;
    System.Action<float> OnValueChanged;

    int lerpIndex;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Update()
    {
        if (lerpIndex == 0)
        {
            if (toLerp == false)
                return;


            //currentObject1.transform.position = Vector3.Lerp(initPos1, finalPos1, lerpTime1);
            float lerpedValue = Mathf.Lerp(startValue, finalValue, lerpTime);
            OnValueChanged.Invoke(lerpedValue);
            if (lerpTime < 1.0f)
            {
                lerpTime += Time.deltaTime / lerpSpeed;
            }
            else
            {
                toLerp = false;
                lerpTime = 0;
                if (lerpComplete != null)
                {
                    lerpComplete.Invoke();
                }
            }
        }
    }
    public void LerpValue(float _startValue, float _finalValue, float speed, System.Action<float> _OnValueChanged, System.Action _lerpComplete = null)
    {
        startValue = _startValue;
        finalValue = _finalValue;
        lerpSpeed = speed;
        lerpTime = 0;
        if (_lerpComplete != null)
            lerpComplete = _lerpComplete;
        else
            lerpComplete = null;

        if (_OnValueChanged != null)
            OnValueChanged = _OnValueChanged;
        else
            OnValueChanged = null;

        toLerp = true;
    }
}
