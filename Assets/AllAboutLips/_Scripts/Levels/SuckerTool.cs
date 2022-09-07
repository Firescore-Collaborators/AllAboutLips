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
        }

        if (Input.GetMouseButtonUp(0))
        {
            held = false;
        }
        lastMousePos = Input.mousePosition;
    }
}
