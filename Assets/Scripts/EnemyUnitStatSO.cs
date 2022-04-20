using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName
= "Enemy Creation/Enemy Units")]
public class EnemyUnitStatSO : ScriptableObject
{
    [Header("Basic Stat")]
    public string unitName;
    public int unitLevel;
    public float currentHp;
    public float currentShd;
    public float currentMp;
    public float currentXp;
    public float maxHp;
    public float maxShd;
    public float maxMp;
    public float maxXp;
    public float manaRegen;
    public float att;
    public float def;
    public float spellRes;
    public float attSpeed;
}
