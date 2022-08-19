using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanManager : MonoBehaviour
{
    public ObjectFollowMouse cleanObject;

    public GameObject sandParticles;
    public GameObject infinity;

    public Material cleanTexture;
    public Texture2D paintTexture;

    void OnDisable()
    {
        LevelObject levelObject = GameManager.Instance.levelObject.GetComponent<LevelObject>();
    }
    void Start()
    {
        Timer.Delay(1.5f, () =>
        {
            infinity.SetActive(true);
        });
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            infinity.SetActive(false);
            cleanObject.enabled=true;
            Timer.Delay(0.3f, () =>
            {
                sandParticles.SetActive(true);
            });
        }

        if(Input.GetMouseButtonUp(0))
        {
            cleanObject.enabled=false;
            sandParticles.SetActive(false);
        }

    }
}
