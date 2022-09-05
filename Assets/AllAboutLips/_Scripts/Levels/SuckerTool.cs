using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckerTool : MonoBehaviour
{
    bool checkForEnter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SuckerPlace"))
        {
            checkForEnter = false;
            GetComponent<ObjectFollowCollider>().Disable();

            LerpObjectPosition.instance.LerpObject(transform, other.transform.GetChild(0).position, 1, () =>
            {
                GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().SuckStart();
            });
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SuckerPlace"))
        {
            GameManager.Instance.GetComponent<SuckerStepState>().stepManager.GetComponent<SuckerManager>().SuckEnd();
            Timer.Delay(1f, () =>
            {
                GetComponent<ObjectFollowCollider>().enabled = false;
            });
        }
    }
}
