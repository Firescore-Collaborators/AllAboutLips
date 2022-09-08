using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    LevelObject levelObject;
    public GameObject confetti, sparkles;
    public int smileBlendKey = 10;
    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        levelObject = GameManager.Instance.levelObject;
        MainCameraController.instance.SetCurrentCamera("Default");
        Timer.Delay(1f, () =>
        {
            confetti.SetActive(true);
        });

        Timer.Delay(1.5f, () =>
        {
            PlayCharacterAnim();
            confetti.SetActive(false);
            sparkles.SetActive(true);
            Timer.Delay(3f, () =>
            {
                sparkles.SetActive(false);
            });
            // LerpFloatValue.instance.LerpValue(0, 60, 0.7f, (var) =>
            // {
            //     levelObject.skinRend.SetBlendShapeWeight(smileBlendKey, var);
            //     GameManager.Instance.gameObject.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().CancelInvoke();
            // });
        });
    }

    void PlayCharacterAnim()
    {
        DisableCharacterRig();
        levelObject.characterAnim.CrossFade("Dance", 1f);
    }

    void DisableCharacterRig()
    {
        levelObject.rig.weight = 0;
    }
}

