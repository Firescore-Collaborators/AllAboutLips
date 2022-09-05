using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SuckerManager : MonoBehaviour
{
    LevelObject levelObject;
    public GameObject suckerTool;
    public SkinnedMeshRenderer suckerRend;
    public Transform suckerStartPos;
    public Image progress;
    public Text stepHeader;
    public float suckAmount = 20f;
    public float suckSpeed = 1f;
    bool canSuck, suckEnable;
    public int blendKey = 19;
    int loopCount;
    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        levelObject = GameManager.Instance.levelObject;
        levelObject.objectRotate.enabled = false;
        stepHeader.text = "Place the pucker";
        suckerTool.transform.position = suckerStartPos.position;
    }

    void Update()
    {
        if (!suckEnable) { return; }
        SetInput();
    }

    void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!canSuck) { return; }

            OnSuck();
        }
    }

    [Button]
    void OnSuck()
    {
        canSuck = false;
        float currentWeight = levelObject.skinRend.GetBlendShapeWeight(blendKey);
        LerpFloatValueBehaviour lerpFloat = suckerTool.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat.LerpValue(100, 0, suckSpeed / 2, (value) =>
        {
            suckerRend.SetBlendShapeWeight(0, value);
        }, () =>
        {
            lerpFloat.LerpValue(0, 100, suckSpeed / 2, (value) =>
            {
                suckerRend.SetBlendShapeWeight(0, value);
            });
        });
        LerpFloatValue.instance.LerpValue(currentWeight, currentWeight + suckAmount, suckSpeed, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(blendKey, value);
            progress.fillAmount = Remap.remap(levelObject.skinRend.GetBlendShapeWeight(blendKey), 0, 100, 0, 1);
        }, () =>
        {
            canSuck = true;
            if (progress.fillAmount == 1)
            {
                RemoveSucker();
            }
        });

    }

    void RemoveSucker()
    {
        suckEnable = false;
        stepHeader.text = "Remove the pucker";
        suckerTool.GetComponent<ObjectFollowCollider>().enabled = true;
        progress.transform.parent.gameObject.SetActive(false);
    }

    public void SuckStart()
    {
        stepHeader.text = "Pucker the lips";
        suckEnable = true;
        canSuck = true;
    }

    public void SuckEnd()
    {
        stepHeader.text = "";
        loopCount = 0;
        LipsExaggerate();
    }

    void LipsExaggerate()
    {
        LerpFloatValue.instance.LerpValue(100, 130, suckSpeed / 4, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(blendKey, value);
        }, () =>
        {
            LerpFloatValue.instance.LerpValue(130, 100, suckSpeed / 4, (value) =>
            {
                levelObject.skinRend.SetBlendShapeWeight(blendKey, value);
            }, () =>
            {
                if (loopCount == 2)
                {
                    LevelEnd();
                }
                else
                {
                    loopCount++;
                    LipsExaggerate();
                }
            });
        });
    }

    void LevelEnd()
    {
        Timer.Delay(3f, () =>
        {
            GameManager.Instance.NextStep();
        });
    }
}
