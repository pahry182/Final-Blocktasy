using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IVisitor<T> where T : UnitBase
{
    void Visit(T unit);
}

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
    public float maxMp = 100f;
    public float maxXp = 100f;
    public float manaRegen = 0;
    public float att = 10f;
    public float mag = 1f;
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

    [Header("Technical")]
    public bool isDead;
    public float xpLoot;

    public void Accept(IVisitor<UnitBase> visitor)
    {
        visitor.Visit(this);
    }

    private void Start()
    {
        
    }

    public IEnumerator PlayerBasicAttack()
    {
        yield return new WaitUntil(() => BattleSceneController.Instance.isAttacking == false);

        Vector2 posLast = transform.position;
        transform.DOMove(new Vector2(transform.position.x - 1.5f, transform.position.y), .6f);

        yield return new WaitForSeconds(1);

        transform.DOMove(posLast, 0.4f);

        yield return new WaitForSeconds(0.5f);

        PhysicalDamage basicAttack = new PhysicalDamage(this, att);
        BattleController.Instance.singleTarget.Accept(basicAttack);

        yield return new WaitForSeconds(2);
        yield return new WaitForEndOfFrame();

        BattleController.Instance.EndTurn();
    }

    public IEnumerator EnemyBasicAttack()
    {
        var sr = GetComponentInChildren<SpriteRenderer>();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(sr.DOColor(Color.black, 0.2f))
          .Append(sr.DOColor(Color.white, 0.2f));

        yield return new WaitForSeconds(0.5f);

        PhysicalDamage basicAttack = new PhysicalDamage(this, att);
        int select = Random.Range(0, BattleController.Instance.player.Length);

        while (BattleController.Instance.player[select].isDead)
        {
            select = Random.Range(0, BattleController.Instance.player.Length);
        }

        BattleController.Instance.player[select].Accept(basicAttack);

        yield return new WaitForSeconds(0.7f);
        yield return new WaitForEndOfFrame();

        BattleController.Instance.EndTurnEnemy();
    }

    public IEnumerator StartDie()
    {
        var sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = Color.red;
        sr.DOFade(0, 1f);

        yield return new WaitForSeconds(0);

        //Destroy(gameObject);
    }
}
