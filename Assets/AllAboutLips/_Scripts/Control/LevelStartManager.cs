using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartManager : MonoBehaviour
{
    public Renderer wallColor,floor;

    public void Init()
    {
        wallColor.materials[0].color = ((LipLevelSO)(GameManager.Instance.CurrentLevel)).wall;
        floor.materials[0].color = ((LipLevelSO)(GameManager.Instance.CurrentLevel)).floor;
        
    }
}
