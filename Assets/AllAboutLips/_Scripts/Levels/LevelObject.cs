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
    public SkinnedMeshRenderer skinRend, outfit;
    public Renderer hair;
    public ObjectRotateInterpolate objectRotate;
    public Rig rig;
    public Animator characterAnim;
    public List<GameObject> hairstyles = new List<GameObject>();

    void Start()
    {
        fracturedMesh.Init();
        outfit.materials[0].color = ((LipLevelSO)(GameManager.Instance.CurrentLevel)).dress;
        hairstyles[(int)((LipLevelSO)(GameManager.Instance.CurrentLevel)).hairstyle].SetActive(true);
        hair = hairstyles[(int)((LipLevelSO)(GameManager.Instance.CurrentLevel)).hairstyle].GetComponent<Renderer>();
        hair.materials[0].SetColor("_BaseColor", ((LipLevelSO)(GameManager.Instance.CurrentLevel)).hair);
    }
}
