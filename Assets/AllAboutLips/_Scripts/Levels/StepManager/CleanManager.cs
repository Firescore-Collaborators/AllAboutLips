using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CleanManager : MonoBehaviour
{
    public static CleanManager instance;
    LevelObject levelObject;
    public LaserController laserController;
    public GameObject laserParticle;
    public Image progressBar;
    public float particleMaxCount;

    bool held;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        levelObject = GameManager.Instance.levelObject;
    }
    void OnEnable()
    {
        Start();
        Init();
    }

    void Init()
    {
        levelObject.fracturedMesh.gameObject.SetActive(true);
        laserController.fracturedMesh = levelObject.fracturedMesh;
    }


    void SetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            held = true;
            OnLaserClickState(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            held = false;
            OnLaserClickState(false);
        }
    }

    void Update()
    {
        SetInput();

        if (!held) { return; }

        progressBar.fillAmount = Remap.remap(levelObject.fracturedMesh.fracturedMesh.Count, 0, particleMaxCount, 1, 0);

        if (progressBar.fillAmount > 0.98f)
        {
            GameManager.Instance.NextStep();
        }
    }

    void OnLaserClickState(bool state)
    {
        laserParticle.SetActive(state);
        laserController.toRayCast = state;
    }


}
