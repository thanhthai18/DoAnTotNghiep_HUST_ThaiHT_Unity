using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataCharacter", menuName = "Data/DataCharacter", order = 0)]
public class DataCharacter : ScriptableObject
{
    public List<Character> listCharacter = new List<Character>();
   
}

[Serializable]
public class Character
{
    public int id;
    public Sprite characterSprite;
    public float speed;
    public float weight;
    public SkillCharacter mySkill;
    public GameObject characterPrefab;
}

[Serializable]
public enum SkillCharacter
{
    SKILL0,
    SKILL1,
    SKILL2,
    SKILL3,
}
