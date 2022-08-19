using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using PaintIn3D;
using CW.Common;

public class ToggleParticles : MonoBehaviour
{
    public LayerMask GuiLayers { set { guiLayers = value; } get { return guiLayers; } }
    [SerializeField] private LayerMask guiLayers = 1 << 5;

    public KeyCode Key { set { key = value; } get { return key; } }
    [SerializeField] private KeyCode key = KeyCode.Mouse0;

    public ParticleSystem Target { set { target = value; } get { return target; } }
    [SerializeField] private ParticleSystem target;

    public bool StoreStates { set { storeStates = value; } get { return storeStates; } }
    [SerializeField] protected bool storeStates = true;
    public ObjectFollowMouse MouseFollow { set { mouseFollow = value; } get { return mouseFollow; } }
    [SerializeField] private ObjectFollowMouse mouseFollow;
	public List<P3dPaintSphere> paintSpheres = new List<P3dPaintSphere>();
    Coroutine play;
    Coroutine stop;

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Release();
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (stop != null) StopCoroutine(stop);
            mouseFollow.enabled = true;
            play = StartCoroutine(Play());
        }


        Release();

    }

    void Release()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseFollow.enabled = false;
            if (play != null) StopCoroutine(play);
            stop = StartCoroutine(Stop());
        }
    }

    IEnumerator Play()
    {
        yield return new WaitForSeconds(0.6f);
        target.gameObject.SetActive(true);
        target.Play();
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.3f);
        target.Stop();
        target.gameObject.SetActive(false);
    }
}

