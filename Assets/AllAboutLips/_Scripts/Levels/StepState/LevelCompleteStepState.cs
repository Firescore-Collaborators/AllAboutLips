using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteStepState : GameStepState
{
    public Transform lerpFinalPos;
    public GameObject levelComplete;

    public GameObject confetti;
    public GameObject sparkle;
    public Transform characterSpawn;
    public List<ParticleSystem> cash = new List<ParticleSystem>();
    public MeshRenderer mesh;
    LevelObject levelObject
    {
        get
        {
            return GameManager.Instance.levelObject;
        }
    }

    public override void OnStepStart()
    {
        base.OnStepStart();
    }
}
