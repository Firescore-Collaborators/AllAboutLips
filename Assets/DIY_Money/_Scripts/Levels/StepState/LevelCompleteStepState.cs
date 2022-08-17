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
    public Transform cashFinalPos;
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
        levelObject.endCharacter.transform.parent = characterSpawn.transform;
        levelObject.endCharacter.transform.localPosition = Vector3.zero;
        levelObject.endCharacter.transform.localRotation = Quaternion.Euler(0, 0, 0);
        levelObject.endCharacter.transform.localScale = Vector3.one;
        levelObject.endCharacter.SetActive(true);
        cash = levelObject.particleSystems;
        //ObjectRotateTween rotateTween = GameManager.Instance.levelObject.GetComponent<ObjectRotateTween>();
        //GameManager.Instance.levelObject.GetComponent<ObjectRotate1>().enabled = true;
        //rotateTween.target = lerpFinalPos;
        //print(lerpFinalPos.position);
        // CameraController.instance.SetCurrentCamera("LevelComplete", 1);
        //     rotateTween.LerpObject(() =>
        //    {

        //    });
        Timer.Delay(0, () =>
        {
            if (levelComplete != null)
            {
                //GameManager.Instance.levelObject.GetComponent<ObjectRotate1>().enabled = true;
                //sparkle.SetActive(true);.
                MainCameraController.instance.SetCurrentCamera("EndScreen", 0);
                ApplyCashMat();
                sparkle.SetActive(false);
                Timer.Delay(2.0f, () =>
                {
                    Instantiate(levelComplete);
                    CashSlap();

                });
            }
        });

        //CameraController.instance.SetCurrentCamera("TopDown", 0);
        //Instantiate(levelComplete);

    }

    [ContextMenu("Test")]
    void ApplyCashMat()
    {
        for (int i = 0; i < cash.Count; i++)
        {
            cash[i].GetComponent<Renderer>().material = GameManager.Instance.levelObject.paintable.GetComponent<Renderer>().materials[1];
        }
    }

    void CashSlap()
    {
        float speed = 0.5f;
        mesh.materials = levelObject.paintable.GetComponent<MeshRenderer>().materials;
        LerpObjectPosition.instance.LerpObject(mesh.transform, cashFinalPos.position, speed);
        LerpObjectRotation.instance.LerpObject(mesh.transform, cashFinalPos.rotation, speed);
        LerpObjectScale.instance.LerpObject(mesh.transform, cashFinalPos.localScale, speed);
    }
}
