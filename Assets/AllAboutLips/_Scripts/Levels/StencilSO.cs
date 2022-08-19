using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StencilMask
{
    public Texture2D paintMask;
    public Texture2D stencilMask;
}

[CreateAssetMenu(fileName = "StencilSO", menuName = "Money/StencilSO")]
public class StencilSO : ScriptableObject
{
    public List<StencilMask> stencilMasks = new List<StencilMask>();
}
