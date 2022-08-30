using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PaintIn3D;
public class PaintManager : MonoBehaviour
{
    //public P3dPaintSphere paintSphere;
    public Transform paintPanel;
    public ScrollRect scrollRect;
    public GameObject paintButtonPrefab;
    public GameObject holdIcon;

    public LipstickData lipstick;
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
    }
    void SpawnButtons()
    {
        for (int i = 0; i < ((PaintStepSO)GameManager.Instance.CurrentStep).colors.colors.Count; i++)
        {
            Button paintButtonObj = Instantiate(paintButtonPrefab, paintPanel).GetComponent<Button>();
            Color color = ((PaintStepSO)GameManager.Instance.CurrentStep).colors.colors[i];
            paintButtonObj.GetComponent<Image>().color = color;
            //paintButtonObj.transform.GetChild(1).GetComponent<Image>().color = color;
            paintButtonObj.onClick.AddListener(() =>
            {
                ChangeColor(paintButtonObj.GetComponent<Image>().color);
                lipstick.gameObject.SetActive(true);
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
        lipstick.gameObject.SetActive(false);
    }

    void ChangeColor(Color color)
    {
        //Change spray can color

        //Change particle color

        //Change paint color
        lipstick.SetColor(color);
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
            lipstick.OnMouseState(false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            scrollRect.gameObject.SetActive(false);
            holdIcon.SetActive(false);
            lipstick.OnMouseState(true);
        }
    }
}
