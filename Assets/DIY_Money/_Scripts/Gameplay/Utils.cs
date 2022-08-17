using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class Utils
{
    public static GameObject SpawnChar(GameObject character, int type, Transform pos)
    {

        GameObject spawnedCharacter = GameObject.Instantiate(character);
        spawnedCharacter.transform.GetChild(type).gameObject.SetActive(true);
        spawnedCharacter.transform.parent = pos;
        spawnedCharacter.transform.Reset();
        return spawnedCharacter;
    }

    public static GameObject spawnGameObject(GameObject gameObject, Transform pos)
    {
        GameObject spawnedObject = GameObject.Instantiate(gameObject);
        spawnedObject.transform.parent = pos;
        spawnedObject.transform.Reset();
        return spawnedObject;
    }

    // public static GameObject SpawnCharacter(this CharacterListSO charList, Transform pos)
    // {
    //     List<GameObject> selectedCharList = new List<GameObject>();
    //     if (Progress.Instance.AvatarGender == 0)
    //     {
    //         selectedCharList = charList.femaleCharacterList;
    //     }
    //     else
    //     {
    //         selectedCharList = charList.maleCharacterList;
    //     }
    //     return Utils.SpawnChar(selectedCharList[0], Progress.Instance.AvatarType, pos);
    // }

    public static GameObject SpawnEfx(Transform spawnPos, GameObject fx, bool onlyOne = false)
    {
        if (onlyOne)
        {
            if (spawnPos.childCount > 0)
            {
                GameObject.Destroy(spawnPos.GetChild(0).gameObject);
            }
        }

        GameObject fxInstance = Utils.spawnGameObject(fx, spawnPos);
        fxInstance.transform.parent = spawnPos;
        //spawnedEfx.Add(fxInstance);
        return fxInstance;
    }

    public static GameObject SpawnEfxWithDestroy(Transform spawnPos, GameObject fx, float destroyTime, bool onlyOne = false)
    {
        GameObject spawnedfx = SpawnEfx(spawnPos, fx, onlyOne);
        GameObject.Destroy(spawnedfx, destroyTime);
        return spawnedfx;
    }

    public static GameObject SpawnEfxWithDestroy(Vector3 spawnPos,Transform parent, GameObject fx, float destroyTime, bool onlyOne = false)
    {
        GameObject spawnedfx = GameObject.Instantiate(fx, parent);
        spawnedfx.transform.position = spawnPos;
        GameObject.Destroy(spawnedfx, destroyTime);
        return spawnedfx;
    }
    public static Sprite NewSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    
}
