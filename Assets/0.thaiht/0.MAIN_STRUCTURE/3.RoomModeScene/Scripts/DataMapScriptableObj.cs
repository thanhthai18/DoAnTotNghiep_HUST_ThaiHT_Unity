using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DataMap", menuName = "Data/DataMap")]
public class DataMapScriptableObj : ScriptableObject
{
    public List<MapInfo> listMapInfo;
    
    
    

    
}

[Serializable]
public class MapInfo
{
    public int id;
    public Sprite spriteMap;
    public Vector2 radiusVector;
    public Vector2 offsetPosition;
    public float brightnessHandle;
}
