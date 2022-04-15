using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    [Header("Basic Stat")]
    public string unitName = "";
    public int unitLevel = 1;
    public float currentHp = 100f;
    public float currentShd = 0f;
    public float currentMp = 100f;
    public float currentXp;
    public float maxHp = 100f;
    public float maxShd = 100f;
    public float maxMp = 100f;
    public float maxXp = 100f;
    public float manaRegen = 0;
    public float att = 10f;
    public float def = 2f;
    public float spellRes = 0.15f;
    public float attSpeed = 1f;
    public float turnGauge = 0f;

    [Header("Growth Stat")]
    public float growthHp;
    public float growthMp;
    public float growthManaReg = 0;
    public float growthAtt;
    public float growthDef;

    private void Start()
    {
        
    }
}
