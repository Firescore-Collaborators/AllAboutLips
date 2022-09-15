using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartManager : MonoBehaviour
{
    public Renderer wallColor;

    

    public void Init()
    {
        wallColor.materials[0].color = ((LipLevelSO)(GameManager.Instance.CurrentLevel)).wall;
    }
}
