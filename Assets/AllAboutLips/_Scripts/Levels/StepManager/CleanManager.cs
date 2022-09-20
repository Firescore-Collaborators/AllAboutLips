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
    public GameObject ftue;
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

    void OnDisable() {
        laserController.fracturedMesh.gameObject.SetActive(false);
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
            ftue.SetActive(false);
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

        if (progressBar.fillAmount >= 0.98f)
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
