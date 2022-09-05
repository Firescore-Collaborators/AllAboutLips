using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CleanStepSO", menuName = "AllAboutLip/StepSO/CleanStepSO", order = 2)]
public class CleanStepSO : StepSO
{

    public override void OnStepEnd()
    {
        Debug.Log("GameStepSO.OnStepEnd()");
    }
}
