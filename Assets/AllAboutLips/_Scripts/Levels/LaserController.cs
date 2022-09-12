using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public LayerMask layerMask;
    public Transform[] directions;
    RaycastHit[] hit;
    public FracturedMesh fracturedMesh;
    public float force = 1f;
    public float time = 0.3f;
    public float torque = 1f;
    public float raycastRadius = 0.3f;
    public bool toRayCast;
    void Update()
    {
        Raycast();
    }

    void Raycast()
    {
        // if (Physics.Raycast(transform.position, (transform.forward * -1).normalized, out hit, 100f, layerMask))
        // {
        //     Fly(hit.collider.GetComponent<Rigidbody>());
        // }

        if (!toRayCast) { return; }
        // if (Physics.SphereCast(transform.position, raycastRadius, (transform.forward * -1).normalized, out hit, 100f, layerMask))
        // {
        //     Fly(hit.collider.GetComponent<Rigidbody>());
        // }
        hit = Physics.SphereCastAll(transform.position, raycastRadius, (transform.forward * -1).normalized, 100f, layerMask);
        foreach(RaycastHit a in hit)
        {
            Fly(a.rigidbody);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, ((transform.forward * -1).normalized) * 100);
        Gizmos.DrawSphere(transform.position, raycastRadius);
    }

    void Fly(Rigidbody other)
    {
        other.isKinematic = false;
        fracturedMesh.fracturedMesh.Remove(other);
        other.gameObject.layer = 0;
        Transform dir = directions[Random.Range(0, directions.Length)];
        other.AddForce(dir.forward * force, ForceMode.Impulse);
        other.AddTorque((dir.forward + dir.right) * torque, ForceMode.Impulse);
        Timer.Delay(time, () =>
        {
            other.useGravity = true;
        });
        Timer.Delay(5.0f, () =>
        {
            Destroy(other.gameObject);
        });
    }
}
