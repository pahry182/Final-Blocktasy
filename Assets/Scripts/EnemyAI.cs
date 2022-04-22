using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private UnitBase ub;
    public int elapsedTurn;
    public bool isSkillCasted;

    private void Awake()
    {
        ub = GetComponent<UnitBase>();
    }

    public void Act()
    {
        int select = Random.Range(0, 2);

        if (elapsedTurn == 1 && select == 1)
        {
            StartCoroutine(BattleController.Instance.currentUnitTurn.EnemyBasicAttack());
        }
        else if (elapsedTurn == 2)
        {
            StartCoroutine(BattleController.Instance.currentUnitTurn.EnemyBasicAttack());
        }
        else
        {
            StartCoroutine(BattleController.Instance.currentUnitTurn.EnemyBasicAttack());
        }
        elapsedTurn++;
    }
}
