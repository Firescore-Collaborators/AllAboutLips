using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CleanManager : MonoBehaviour
{
    LevelObject levelObject;
    public LaserController laserController;
    public GameObject laserParticle;
    public Image progressBar;
    public float particleMaxCount;

    bool held;


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

        if (progressBar.fillAmount >= 1f)
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
