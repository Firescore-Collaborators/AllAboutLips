using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PaintStepSO", menuName = "Money/StepSO/PaintStepSO")]
public class PaintStepSO : StepSO
{
    public ColorSO colors;
    public override void OnStepEnd()
    {
        base.OnStepEnd();
    }
}
