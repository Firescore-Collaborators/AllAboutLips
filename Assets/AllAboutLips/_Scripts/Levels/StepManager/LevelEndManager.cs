using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndManager : MonoBehaviour
{
    LevelObject levelObject;
    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        levelObject = GameManager.Instance.levelObject;
        MainCameraController.instance.SetCurrentCamera("Default");
        Timer.Delay(2f, () =>
        {
            PlayCharacterAnim();
        });
    }

    void PlayCharacterAnim()
    {
        DisableCharacterRig();
        levelObject.characterAnim.Play("Dance");
    }

    void DisableCharacterRig()
    {
        levelObject.rig.weight = 0;
    }
}

