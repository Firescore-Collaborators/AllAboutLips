using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
public class LevelObject : MonoBehaviour
{
    public P3dPaintableTexture lipsPaintable;
    public FracturedMesh fracturedMesh;
    public GameObject stencil;
    public SkinnedMeshRenderer skinRend;
    public ObjectRotateInterpolate objectRotate;
    void Start()
    {
        fracturedMesh.Init();
    }
}
