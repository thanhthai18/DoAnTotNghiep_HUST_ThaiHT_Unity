using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DataCharacter", menuName = "Data/DataCharacter", order = 0)]
public class DataCharacter : ScriptableObject
{
    public List<CharacterData> listCharacter = new List<CharacterData>();
   
}

[Serializable]
public class CharacterData
{
    public int id;
    public Sprite characterSprite;
    public float weight;
    public float moveSpeed;
    public float dragRigidbody;
    public float dashingTime;
    public float dashDistance;
    public float pushForce;
    public float countDownDash;
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
