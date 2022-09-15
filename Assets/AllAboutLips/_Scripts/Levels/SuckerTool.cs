using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerTool : MonoBehaviour
{
    bool held;
    bool checkForEnter = true;
    public bool checkForPull;
    float currentPullPercent = 0;
    public float pullAddAmount = 20;

    Vector3 lastMousePos, dragPos;
    SuckerManager suckerManager;
    LevelObject levelObject;
    public Transform poutPos;

    void Start()
    {
        suckerManager = GameManager.Instance.gameObject.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>();
        levelObject = GameManager.Instance.levelObject;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SuckerPlace"))
        {
            if (!checkForEnter) return;

            checkForEnter = false;
            GetComponent<ObjectFollowCollider>().Disable();
            GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().Pout();
            GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().placeFute.SetActive(false);
            Timer.Delay(1.5f,()=>
            {
                GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().TapFtue.SetActive(true);
            });
            LerpObjectPosition.instance.LerpObject(transform, poutPos.position, 1, () =>
            {
                GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().SuckStart();
            });
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SuckerPlace"))
        {
            // GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().SuckEnd();
            // Timer.Delay(1f, () =>
            // {
            //     GetComponent<ObjectFollowCollider>().enabled = false;
            // });
        }
    }

    void Update()
    {
        SetInput();
        if (!checkForPull) return;
        if (!held) return;
        if (dragPos == Vector3.zero) return;

        currentPullPercent += Time.deltaTime * pullAddAmount;
        levelObject.skinRend.SetBlendShapeWeight(suckerManager.pullBlendKey, currentPullPercent);

        if (currentPullPercent > 100)
        {
            print("pulled");
            checkForPull = false;
            OpenEyes();
            LerpFloatValue.instance.LerpValue(100, 0, 0.2f, (value) =>
            {
                levelObject.skinRend.SetBlendShapeWeight(suckerManager.pullBlendKey, value);

            }, () =>
            {
                suckerManager.SuckEnd();
            });
        }
    }

    void SetInput()
    {
        dragPos = Input.mousePosition - lastMousePos;
        if (Input.GetMouseButtonDown(0))
        {
            held = true;
            if(checkForPull)
            CloseEyes();
        }

        if (Input.GetMouseButtonUp(0))
        {
            held = false;
            if(checkForPull)
            OpenEyes();

        }
        lastMousePos = Input.mousePosition;
    }

    void CloseEyes()
    {
        GameManager.Instance.gameObject.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().CancelInvoke();
        LerpFloatValueBehaviour lerpFloat2 = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        float startValue = levelObject.skinRend.GetBlendShapeWeight(1);
        lerpFloat2.LerpValue(startValue, 100, 0.3f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(1, value);
            levelObject.skinRend.SetBlendShapeWeight(8, value);
            levelObject.skinRend.SetBlendShapeWeight(23, value);
        });
    }

    void OpenEyes()
    {
        LerpFloatValueBehaviour lerpFloat2 = levelObject.skinRend.gameObject.AddComponent<LerpFloatValueBehaviour>();
        float startValue = levelObject.skinRend.GetBlendShapeWeight(1);
        lerpFloat2.LerpValue(startValue, 0, 0.3f, (value) =>
        {
            levelObject.skinRend.SetBlendShapeWeight(1, value);
            levelObject.skinRend.SetBlendShapeWeight(8, value);
            levelObject.skinRend.SetBlendShapeWeight(23, value);
        });
    }
}
