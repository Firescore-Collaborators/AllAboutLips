using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PaintIn3D;
public class LipstickData : MonoBehaviour
{
    public List<P3dPaintSphere> paintSpheres = new List<P3dPaintSphere>();
    public List<Renderer> rends = new List<Renderer>();
    public ObjectFollowMouseVertical objectFollow;
    public ObjectRotateInterpolate rotate;

    void Start()
    {
        SetColorFromRend();
    }

    void SetColorFromRend()
    {
        Color color = rends[0].material.color;
        for (int i = 0; i < paintSpheres.Count; i++)
        {
            paintSpheres[i].Color = color;
        }
    }

    public void SetColor(Color color)
    {
        for (int i = 0; i < paintSpheres.Count; i++)
        {
            paintSpheres[i].Color = color;
        }
        for (int j = 0; j < rends.Count; j++)
        {
            rends[j].material.color = color;
        }
    }

    public void OnMouseState(bool state)
    {
        objectFollow.enabled = state;
        rotate.enabled = state;
        for(int i = 0; i < paintSpheres.Count; i++)
        {
            paintSpheres[i].gameObject.SetActive(state);
        }
    }

}
