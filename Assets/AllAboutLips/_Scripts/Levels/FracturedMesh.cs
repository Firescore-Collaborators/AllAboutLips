using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedMesh : MonoBehaviour
{
    public List<Rigidbody> fracturedMesh = new List<Rigidbody>();

    public void Init()
    {
        fracturedMesh.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            AddToListInChild(transform.GetChild(i));
        }

        (GameManager.Instance.GetComponent<CleanStepState>().stepManager.GetComponent<CleanManager>().particleMaxCount) = fracturedMesh.Count;
    }

    void AddToListInChild(Transform parent)
    {
        if (parent.childCount == 0) { return; }

        for (int i = 0; i < parent.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            Rigidbody rb = child.AddComponent<Rigidbody>();
            child.AddComponent<SphereCollider>();
            fracturedMesh.Add(rb);
            rb.isKinematic = true;
            rb.useGravity = false;
            child.layer = 6;
            AddToListInChild(child.transform);
        }
    }
}
