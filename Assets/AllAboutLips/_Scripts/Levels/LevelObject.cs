using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
using UnityEngine.Animations.Rigging;

public class LevelObject : MonoBehaviour
{
    public P3dPaintableTexture lipsPaintable;
    public FracturedMesh fracturedMesh;
    public GameObject stencil;
    public SkinnedMeshRenderer skinRend, hair;
    public ObjectRotateInterpolate objectRotate;
    public Rig rig;
    public Animator characterAnim;

    void Start()
    {
        fracturedMesh.Init();
        hair.materials[0].SetColor("_BaseColor", ((LipLevelSO)(GameManager.Instance.CurrentLevel)).hair);
    }
}
