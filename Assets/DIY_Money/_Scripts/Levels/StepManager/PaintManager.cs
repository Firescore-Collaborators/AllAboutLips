using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PaintManager : MonoBehaviour
{
    //public P3dPaintSphere paintSphere;
    public Transform paintPanel;
    public ScrollRect scrollRect;
    public GameObject paintButtonPrefab;
    public GameObject holdIcon;

    public GameObject paintSpray;
    public List<GameObject> paintButton = new List<GameObject>();

    LevelObject levelObject;

    void Start()
    {
        levelObject = GameManager.Instance.levelObject;
    }

    void OnEnable()
    {
        Start();
        Init();
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
        levelObject.paintable.enabled = true;
        levelObject.paintable2.enabled = false;
    }
    void SpawnButtons()
    {
        for (int i = 0; i < ((PaintStepSO)GameManager.Instance.CurrentStep).colors.colors.Count; i++)
        {
            Button paintButtonObj = Instantiate(paintButtonPrefab, paintPanel).GetComponent<Button>();
            Color color = ((PaintStepSO)GameManager.Instance.CurrentStep).colors.colors[i];
            paintButtonObj.transform.GetChild(0).GetComponent<Image>().color = color;
            //paintButtonObj.transform.GetChild(1).GetComponent<Image>().color = color;
            paintButtonObj.onClick.AddListener(() =>
            {
                ChangeColor(paintButtonObj.transform.GetChild(0).GetComponent<Image>().color);
                paintSpray.SetActive(true);
            });
            paintButton.Add(paintButtonObj.gameObject);
        }
    }
    void Reset()
    {
        for (int i = 0; i < paintButton.Count; i++)
        {
            Destroy(paintButton[i]);
        }
        paintButton.Clear();
    }

    void ChangeColor(Color color)
    {
        //Change spray can color
        MeshRenderer mesh = paintSpray.GetComponent<MeshRenderer>();
        Material[] mats = mesh.materials;
        mats[0].color = color;
        mesh.materials = mats;

        //Change particle color
        ToggleParticles toggleParticles = paintSpray.GetComponent<ToggleParticles>();
        GameObject particles = toggleParticles.Target.gameObject;
        ParticleSystem.MainModule settings = particles.GetComponent<ParticleSystem>().main;
        settings.startColor = color;

        //Change paint color
        for (int i = 0; i < toggleParticles.paintSpheres.Count; i++)
        {
            toggleParticles.paintSpheres[i].Color = color;
        }
        //paintSphere.Color = color;
    }

    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            scrollRect.gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            scrollRect.gameObject.SetActive(false);
            holdIcon.SetActive(false);
        }
    }
}
