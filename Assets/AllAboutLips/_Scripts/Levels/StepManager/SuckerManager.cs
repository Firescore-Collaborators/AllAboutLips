using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class SuckerManager : MonoBehaviour
{
    public Gradient skinColor;
    LevelObject levelObject;
    public GameObject suckerTool;
    public SkinnedMeshRenderer suckerRend;
    public Transform suckerStartPos, suckFinalPos;
    public Image progress;
    public Text stepHeader;
    public GameObject particleEfx, heartPfx;
    public float suckAmount = 20f;
    public float suckSpeed = 1f;
    bool canSuck, suckEnable;
    public int swollblendKey = 19, poutBlendkey = 20, pullBlendKey = 21, expressionBlendKey = 8, blinkBlendKey = 1, chinSuckBlendKey = 24;
    int loopCount;
    public bool isFailVideo;

    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        levelObject = GameManager.Instance.levelObject;
        levelObject.stencil.SetActive(false);
        levelObject.fracturedMesh.gameObject.SetActive(false);
        levelObject.objectRotate.enabled = false;
        stepHeader.text = "Place the pucker";
        suckerTool.transform.position = suckerStartPos.position;
        LerpObjectLocalRotation.instance.LerpObject(levelObject.objectRotate.transform, Quaternion.Euler(Vector3.zero), 0.3f);
        InvokeRepeating("Blink", 2, 5);
        //levelObject.objectRotate.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    void OnDisable()
    {
        CancelInvoke();
        levelObject.objectRotate.enabled = true;
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
        CancelInvoke();
        canSuck = false;
        float currentWeight = levelObject.skinRend.GetBlendShapeWeight(swollblendKey);
        //Tool
        LerpFloatValueBehaviour lerpFloat = suckerTool.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat.LerpValue(100, 0, suckSpeed / 2, (value) =>
        {
            suckerRend.SetBlendShapeWeight(0, value);
        }, () =>
        {
            lerpFloat.LerpValue(0, 100, suckSpeed / 2, (value) =>
            {
                suckerRend.SetBlendShapeWeight(0, value);
            }, () =>
            {
                Destroy(lerpFloat);
            });
        });

        //ChinSuck
        LerpFloatValueBehaviour lerpFloat3 = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat3.LerpValue(0, 100, suckSpeed / 2, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(chinSuckBlendKey, value);
        }, () =>
        {
            lerpFloat3.LerpValue(100, 0, suckSpeed / 2, (value) =>
            {
                levelObject.skinRend.SetBlendShapeWeight(chinSuckBlendKey, value);
            }, () =>
            {
                Destroy(lerpFloat3);
            });
        });

        // Eyes close
        LerpFloatValueBehaviour lerpFloat2 = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat2.LerpValue(0, 100, 0.3f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(blinkBlendKey, value);
        }, () =>
        {
            Timer.Delay(0.6f, () =>
            {
                lerpFloat2.LerpValue(100, 0, 0.2f, (value) =>
                {
                    levelObject.skinRend.SetBlendShapeWeight(blinkBlendKey, value);
                }, () =>
                {
                    Destroy(lerpFloat2);
                    InvokeRepeating("Blink", 3, 5);
                });
            });
        });

        LerpFloatValueBehaviour lerpFloat1 = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat1.LerpValue(0, 100, suckSpeed / 4, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(expressionBlendKey, value);
        }, () =>
        {
            Timer.Delay(0.6f, () =>
            {
                lerpFloat1.LerpValue(100, 0, suckSpeed / 4, (value) =>
                {
                    levelObject.skinRend.SetBlendShapeWeight(expressionBlendKey, value);
                }, () =>
                {
                    Destroy(lerpFloat1);
                });
            });

        });


        //Lips
        LerpFloatValue.instance.LerpValue(currentWeight, currentWeight + suckAmount, suckSpeed, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(swollblendKey, value);
            progress.fillAmount = Remap.remap(levelObject.skinRend.GetBlendShapeWeight(swollblendKey), 0, 100, 0, 1);
            if (isFailVideo)
            {
                levelObject.skinRend.materials[0].color = skinColor.Evaluate(progress.fillAmount);
            }
        }, () =>
        {
            canSuck = true;
            if (progress.fillAmount == 1)
            {
                RemoveSucker();
            }
        });

    }

    public void Blink()
    {
        LerpFloatValueBehaviour lerpFloat = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        lerpFloat.LerpValue(0, 100, 0.3f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(blinkBlendKey, value);
        }, () =>
        {
            Timer.Delay(0.1f, () =>
            {
                lerpFloat.LerpValue(100, 0, 0.1f, (value) =>
                {
                    levelObject.skinRend.SetBlendShapeWeight(blinkBlendKey, value);
                }, () =>
                {
                    Destroy(lerpFloat);
                });
            });
        });
    }

    public void Pout()
    {
        LerpFloatValue.instance.LerpValue(0, 100, 1, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(poutBlendkey, value);
        }, () =>
        {
            particleEfx.SetActive(true);
            Timer.Delay(1f, () =>
            {
                particleEfx.SetActive(false);
            });
            // LerpFloatValue.instance.LerpValue(100, 0, 0.75f, (value) =>
            // {
            //     levelObject.skinRend.SetBlendShapeWeight(poutBlendkey, value);
            // });
            // LerpObjectPosition.instance.LerpObject(suckerTool.transform,pos.position,0.75f);            
        });
    }
    void RemoveSucker()
    {
        LerpFloatValue.instance.LerpValue(100, 0, 0.25f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(poutBlendkey, value);
        });
        LerpObjectPosition.instance.LerpObject(suckerTool.transform, suckFinalPos.position, 0.25f);

        suckEnable = false;
        stepHeader.text = "Remove the pucker";
        suckerTool.GetComponent<SuckerTool>().checkForPull = true;
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
        LipsPull();

        if (isFailVideo)
        {
            LerpFloatValueBehaviour lerpFloat = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
            lerpFloat.LerpValue(1, 0, 1f, (var) =>
            {
                levelObject.skinRend.materials[0].color = skinColor.Evaluate(var);
            });
        }

        Timer.Delay(0, () =>
        {
            particleEfx.SetActive(true);
            Timer.Delay(1f, () =>
            {
                particleEfx.SetActive(false);
            });
            LipsExaggerate();
        });
    }

    void LipsPull()
    {
        LerpFloatValue.instance.LerpValue(0, 100, 0.1f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(pullBlendKey, value);
        }, () =>
        {
            LerpFloatValue.instance.LerpValue(100, 0, 0.1f, (value) =>
            {
                levelObject.skinRend.SetBlendShapeWeight(pullBlendKey, value);
            });
        });
    }
    void LipsExaggerate()
    {

        LerpFloatValue.instance.LerpValue(100, 130, suckSpeed / 4, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(swollblendKey, value);
        }, () =>
        {
            LerpFloatValue.instance.LerpValue(130, 100, suckSpeed / 4, (value) =>
            {
                levelObject.skinRend.SetBlendShapeWeight(swollblendKey, value);
            }, () =>
            {
                if (loopCount == 2)
                {
                    heartPfx.SetActive(true);
                    Timer.Delay(2f, () =>
                    {
                        heartPfx.SetActive(false);
                    });
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
