using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Hairstyle{
    hairstyle1, hairstyle2, hairstyle3, hairstyle4
}

[CreateAssetMenu(fileName = "LipLevelSO", menuName = "AllAboutLip/LevelSO", order = 1)]
public class LipLevelSO : LevelSO
{
    public Color hair, wall, floor, dress;
    public Hairstyle hairstyle; 

}
