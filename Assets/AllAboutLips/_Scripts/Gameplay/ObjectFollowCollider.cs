using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowCollider : MonoBehaviour
{
    public LayerMask objectLayer;
    public LayerMask moveLayer;
    Ray ray;
    RaycastHit hit;

    GameObject hitObject;
    Vector3 offset;
    public bool shouldOffset;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        SetInput();
        MoveAlongCollider();
    }

    void SetInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(ray, out hit, 100f, objectLayer))
            {
                hitObject = hit.collider.gameObject;
                if(!shouldOffset) return;
                offset = hit.point - hitObject.transform.position;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            hitObject = null;
        }
    }

    void MoveAlongCollider()
    {
        if (hitObject == null) { return; }

        if (Physics.Raycast(ray, out hit, 100f, moveLayer))
        {
            hitObject.transform.position = hit.point + offset;
        }
    }

    public void Disable()
    {
        hitObject = null;
        this.enabled = false;
    }
}
