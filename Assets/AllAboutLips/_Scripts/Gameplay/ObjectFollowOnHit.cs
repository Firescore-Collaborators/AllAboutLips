using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectFollowOnHit : MonoBehaviour
{
    public LayerMask colliderLayer;
    RaycastHit hit;
    Ray ray;
    Vector3 offset;
    public bool shouldOffset;
    public bool toMove;
    bool held;
    bool offsetCalculated;
    Vector3 hitPos;
    void Update()
    {
        if (!toMove) { return; }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SetInput();
        MoveAlongCollider();
    }


    void SetInput()
    {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
#endif

#if UNITY_ANDROID
        if (EventSystem.current.IsPointerOverGameObject(0)) { return; }
#endif


        if (Input.GetMouseButtonDown(0))
        {
            held = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            held = false;
        }
    }

    void MoveAlongCollider()
    {
        if (!held) { return; }

        if (Physics.Raycast(ray, out hit, 100f, colliderLayer))
        {
            // if (!offsetCalculated)
            // {
            //     offset = transform.position - hit.point;
            // }
            hitPos = hit.point;
            transform.position = hit.point;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPos, 0.1f);
        Gizmos.DrawRay(ray);
    }
}
