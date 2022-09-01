using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour
{
    LevelObject levelObject;
    public LaserController laserController;
    
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
}
