using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    public LevelObject levelObject;
    public GameObject confetti, sparkles, hearts;
    public int smileBlendKey = 10;
    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        //levelObject = GameManager.Instance.levelObject;
        InvokeRepeating("Blink", 2, 5);
        MainCameraController.instance.SetCurrentCamera("Default");
        Timer.Delay(1f, () =>
        {
            //confetti.SetActive(true);
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

        });

        Timer.Delay(5f, () =>
        {
            //MainCameraController.instance.SetCurrentCamera("Final Camera");
            //     LerpFloatValue.instance.LerpValue(0, 60, 0.7f, (var) =>
            //     {
            //         levelObject.skinRend.SetBlendShapeWeight(smileBlendKey, var);
            //         GameManager.Instance.gameObject.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().CancelInvoke();
            //     });
        });
    }

    void PlayCharacterAnim()
    {
        levelObject.characterAnim.SetLayerWeight(1,100);
        levelObject.characterAnim.SetBool("final", true);
        MainCameraController.instance.SetCurrentCamera("Final Camera");
        Timer.Delay(2, () =>
        {
            hearts.SetActive(true);
        });

        Timer.Delay(5f, () =>
        {
            hearts.SetActive(false);
        });
        Timer.Delay(1.2f, () =>
        {
            DisableCharacterRig();

        });
    }

    void DisableCharacterRig()
    {
        levelObject.rig.weight = 0;
    }

    public void Blink()
    {
        LerpFloatValueBehaviour lerpFloat = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat.LerpValue(0, 100, 0.3f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(1, value);
        }, () =>
        {
            Timer.Delay(0.1f, () =>
            {
                lerpFloat.LerpValue(100, 0, 0.1f, (value) =>
                {
                    levelObject.skinRend.SetBlendShapeWeight(1, value);
                }, () =>
                {
                    Destroy(lerpFloat);
                });
            });
        });
    }


}

