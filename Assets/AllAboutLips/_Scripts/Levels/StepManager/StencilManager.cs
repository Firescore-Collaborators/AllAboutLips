using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PaintIn3D;
using System.Linq;

public class StencilManager : MonoBehaviour
{
    public Transform paintPanel;
    public ScrollRect scrollRect;
    public GameObject paintButtonPrefab;
    public LipstickData lipstick;
    public GameObject button;
    public GameObject stencil;
    LevelObject levelObject;
    public List<GameObject> paintButton = new List<GameObject>();


    int currentStencil = 0;

    private void Start()
    {
        levelObject = GameManager.Instance.levelObject;
        stencil = levelObject.stencil;
    }
    void OnEnable()
    {
        Start();
        Init();
        SetStencil();
        //ApplyMask();
    }

    void OnDisable()
    {
        Reset();
    }

    void Init()
    {
        SpawnButtons();
        SpawnButtons();
        scrollRect.horizontalNormalizedPosition = 0;
        Canvas.ForceUpdateCanvases();
        //AddOverlay();
        // levelObject.paintable.enabled = false;
        // levelObject.paintable2.enabled = true;
        // levelObject.paintable.Deactivate();
        // levelObject.paintable2.Activate();
    }

    void SpawnButtons()
    {
        for (int i = 0; i < ((StencilStepSo)GameManager.Instance.CurrentStep).colors.colors.Count; i++)
        {
            Button paintButtonObj = Instantiate(paintButtonPrefab, paintPanel).GetComponent<Button>();
            Color color = ((StencilStepSo)GameManager.Instance.CurrentStep).colors.colors[i];
            paintButtonObj.GetComponent<Image>().color = color;
            //paintButtonObj.transform.GetChild(1).GetComponent<Image>().color = ((StencilStepSo)GameManager.Instance.CurrentStep).colors.colors[i];
            paintButtonObj.onClick.AddListener(() =>
            {
                ChangeColor(paintButtonObj.GetComponent<Image>().color);
                lipstick.gameObject.SetActive(true);
            });
            paintButton.Add(paintButtonObj.gameObject);
        }
    }

    void AddOverlay()
    {
        //if (levelObject.overlayAdded) return;

        //MeshRenderer mesh = levelObject.paintable.GetComponent<MeshRenderer>();
        //List<Material> mats = (mesh.materials).ToList();
        //Material overlayMat = levelObject.overlayMat;
        //mats.Add(overlayMat);
        //mesh.materials = mats.ToArray();
        //levelObject.overlayAdded = true;
    }

    void Reset()
    {
        //levelObject.paintable2.LocalMaskTexture = null;
        levelObject.lipsPaintable.LocalMaskTexture = null;
        lipstick.gameObject.SetActive(false);
        for (int i = 0; i < paintButton.Count; i++)
        {
            Destroy(paintButton[i]);
        }
        paintButton.Clear();
    }

    void ChangeColor(Color color)
    {
        //Change spray can color
        

        //Change particle color

        //Change paint color
        lipstick.SetColor(color);
        //paintSphere.Color = color;
    }
    void ApplyMask()
    {
        //levelObject.stencil.LocalMaskTexture = mask;
    }

    void SetStencil()
    {
        StencilMask stencilMask = ((StencilStepSo)GameManager.Instance.CurrentStep).stencilSO.stencilMasks[currentStencil];
        P3dPaintableTexture stencilPaintable = stencil.GetComponent<P3dPaintableTexture>();
        stencilPaintable.LocalMaskTexture = stencilMask.stencilMask;
        stencilPaintable.Texture = stencilMask.stencilAlbedo;
        stencil.GetComponent<Renderer>().material.SetTexture("_Albedo",stencilMask.stencilAlbedo);
        levelObject.lipsPaintable.LocalMaskTexture = stencilMask.paintMask;
        stencil.SetActive(true);
        stencilPaintable.Deactivate();
        stencilPaintable.Activate();
        //stencil = Instantiate(levelObject.stencil);
        //stencil.transform.parent = levelObject.stencil.transform.parent;
        //stencil.transform.position = levelObject.stencil.transform.position;
        //StencilData stencilData = stencil.GetComponent<StencilData>();
        //P3dPaintableTexture stencilPaint = stencilData.stencilPaint.GetComponent<P3dPaintableTexture>();
        //P3dPaintableTexture stencilBone = stencilData.stencilBone.GetComponent<P3dPaintableTexture>();
        //stencilMesh.material.mainTexture = stencilMask.stencilMask;
        // stencilPaint.Texture = stencilMask.stencilMask;
        // stencilBone.Texture = stencilMask.stencilMask;
        // stencilPaint.LocalMaskTexture = stencilMask.stencilMask;
        // stencilBone.LocalMaskTexture = stencilMask.stencilMask;
        //levelObject.paintable2.LocalMaskTexture = stencilMask.paintMask;
        // stencil.SetActive(true);
        // stencilData.stencilBone.SetActive(true);
        // stencilData.anim.Play("Apply");
        Timer.Delay(1f, () =>
        {
            // stencilData.stencilBone.SetActive(false);
            // stencilData.stencilPaint.SetActive(true);
        });
        //stencil.GetComponent<Animator>().Play("Apply");
    }

    void RemoveStencil(System.Action callback = null)
    {
        //stencil.GetComponent<Animator>().Play("Remove");
        //StencilData stencilData = stencil.GetComponent<StencilData>();
        //stencilData.stencilBone.SetActive(true);
        //stencilData.stencilPaint.SetActive(false);
        //stencilData.stencilBone.GetComponent<Renderer>().materials = stencilData.stencilPaint.GetComponent<MeshRenderer>().materials;
        //stencil.GetComponent<Animator>().Play("Remove");
        stencil.SetActive(false);
        Timer.Delay(1.0f, () =>
        {
            //stencil.SetActive(false);
            callback();
        });
    }

    public void StepEnd()
    {
        RemoveStencil();
        Timer.Delay(1.0f, () =>
        {
            GameManager.Instance.NextStep();
        });
    }

    public void NextStep()
    {
        currentStencil++;
        if (currentStencil >= ((StencilStepSo)GameManager.Instance.CurrentStep).stencilSO.stencilMasks.Count)
        {
            button.SetActive(false);
            StepEnd();
        }
        else
        {
            RemoveStencil(() =>
            {
                SetStencil();
            });
        }
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            scrollRect.gameObject.SetActive(false);
            lipstick.OnMouseState(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            scrollRect.gameObject.SetActive(true);
            lipstick.OnMouseState(false);
        }

    }
}

