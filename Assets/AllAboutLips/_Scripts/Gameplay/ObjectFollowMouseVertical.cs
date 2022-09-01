using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowMouseVertical : MonoBehaviour
{
    private Camera cam;
    Ray ray;
    public float distance;
    public Vector3 cameraAxis;

    Vector3 newPoint;
    Vector3 defValue;

    RaycastHit hit;
    public float valueZ;
    public Vector3 offset;
    public bool follow, isHover;
    public float hoverSpeed = .1f;
    public float paintValue, hoverValue;
    bool isHeld, isReached;

    private void OnEnable()
    {

        Start();
        ray = cam.ScreenPointToRay(Input.mousePosition);

        newPoint = ray.GetPoint(distance);
        if (valueZ == 0)
        {
            valueZ = transform.position.z;
        }
        //offset = newPoint - transform.position ;
    }
    void Start()
    {
        cam = Camera.main;
        if (distance == 0)
            distance = Vector3.Distance(cam.GetComponent<Transform>().position, transform.position);
        defValue = transform.position;
        hoverValue = valueZ;
    }
    private void Update()
    {
        SetInput();
        ObjectHover();
        if (!follow)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, valueZ);
            return;
        }

        ray = cam.ScreenPointToRay(Input.mousePosition);

        newPoint = ray.GetPoint(distance);

        if (cameraAxis.x == 1)
        {
            newPoint.x = defValue.x;
        }
        else if (cameraAxis.z == 1)
        {
            newPoint.z = defValue.z;
        }
        else if (cameraAxis.y == 1)
        {
            newPoint.y = defValue.y;
        }
        //transform.position = newPoint;
        transform.position = new Vector3(newPoint.x, newPoint.y, valueZ);
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + offset, 0.01f);
    }

    void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isHeld = true;
            isReached = false;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
            isReached = false;
        }
    }

    void ObjectHover()
    {
        if (!isHover) { return; }
        if (isHeld)
        {
            if (isReached) { return; }

            valueZ -= Time.deltaTime * hoverSpeed;
            if (valueZ <= paintValue)
            {
                isReached = true;
                valueZ = paintValue;
            }
        }
        else
        {
            if (isReached) { return; }

            valueZ += Time.deltaTime * hoverSpeed;
            if (valueZ >= hoverValue)
            {
                isReached = true;
                valueZ = hoverValue;
            }
        }
    }

}
