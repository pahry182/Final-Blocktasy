using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BattleSceneController : UIController
{
    public static BattleSceneController Instance { get; private set; }

    public CanvasGroup battlePanel, winPanel;
    public CanvasGroup deck, commandHUD, announceHUD, magicHUD;
    public TextMeshProUGUI announceText;
    public GameObject textDamage, targetAllButton, itemNameList, itemCountList;
    public GameObject prefabItemName, prefabItemCount;
    public GameObject[] spellButtons;

    private BasicSpell basicSpell;
    private string effectName;
    public bool isAttacking;
    public bool isTargetAllEnemy;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        battlePanel.gameObject.SetActive(true);
        commandHUD.gameObject.SetActive(false);
        announceHUD.gameObject.SetActive(false);
        winPanel.gameObject.SetActive(false);
        foreach (Transform item in itemNameList.transform)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in itemCountList.transform)
        {
            Destroy(item.gameObject);
        }
    }

    private void Update()
    {
        CheckSelectAttack();
    }

    public void OpenPlayerCommand()
    {
        StartCoroutine(FadeIn(commandHUD, 0.1f));
    }

    public void ClosePlayerCommand()
    {
        StartCoroutine(FadeOut(commandHUD, 0.1f));
    }

    private void OpenAnnounceHUD(string send)
    {
        announceText.text = send;
        StartCoroutine(FadeIn(announceHUD, 0.1f));
    }

    private void CloseAnnounceHUD()
    {
        StartCoroutine(FadeOut(announceHUD, 0.1f));
    }

    public void AttackButton()
    {
        isAttacking = true;
        OpenAnnounceHUD("Choose Enemy to Attack");
        StartCoroutine(BattleController.Instance.currentUnitTurn.PlayerBasicAttack());
        ClosePlayerCommand();
    }

    private void CheckSelectAttack()
    {
        if (Input.GetMouseButtonDown(0) && isAttacking)
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);
            if (hit)
            {
                BattleController.Instance.singleTarget = hit.transform.gameObject.GetComponentInParent<UnitBase>();
                if (hit.transform.gameObject.tag == "Enemy" && !BattleController.Instance.singleTarget.isDead)
                {
                    isAttacking = false;
                    Debug.Log("Enemy Selected");
                    CloseAnnounceHUD();
                    targetAllButton.SetActive(false);
                    GameManager.Instance.PlaySfx("Button");
                }
                else
                {
                    Debug.Log("Please select the Enemy");
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }
    }

    public void SelectAllTarget()
    {
        isAttacking = false;
        isTargetAllEnemy = true;
        targetAllButton.SetActive(false);
        CloseAnnounceHUD();
    }

    public void ShowTextDamage(int amount, Vector3 pos)
    {
        var temp = Instantiate(textDamage, pos, Quaternion.identity);
        temp.GetComponent<TextMeshPro>().text = amount.ToString();
        temp.transform.DOJump(temp.transform.position, 1, 1, 0.5f);
        Destroy(temp, 1.5f);
    }

    public void WonHUD()
    {
        OpenAnnounceHUD("Victory!");
        StartCoroutine(ProceedWinning());

    }

    public void LoseHUD()
    {
        OpenAnnounceHUD("Defeat");
    }

    private IEnumerator ProceedWinning()
    {
        yield return new WaitForSeconds(1.8f);

        CloseAnnounceHUD();
        StartCoroutine(FadeIn(winPanel, 0.3f));

        yield return new WaitForSeconds(0.48f);

        for (int i = 0; i < BattleController.Instance.lootGainedDict.Count; i++)
        {
            var temp = Instantiate(prefabItemName, itemNameList.transform);
            temp.GetComponent<TextMeshProUGUI>().text = BattleController.EXP_GAINED;

            yield return new WaitForSeconds(0.48f);

            var temp2 = Instantiate(prefabItemCount, itemCountList.transform);
            temp2.GetComponent<TextMeshProUGUI>().text = BattleController.Instance.lootGainedDict[BattleController.EXP_GAINED].ToString();
        }
        
    }

    public void OpenMagicCommand()
    {
        StartCoroutine(FadeIn(magicHUD, 0.15f));
        foreach (var item in spellButtons)
        {
            item.SetActive(false);
            if (BattleController.Instance.currentUnitTurn.learnedSpell.Contains(item.name))
            {
                item.SetActive(true);
            }
        }
    }

    public void CloseMagicCommand()
    {
        StartCoroutine(FadeOut(magicHUD, 0.15f));
    }

    public void FireSpellButton()
    {
        basicSpell = new BasicSpell(BattleController.Instance.currentUnitTurn, ElementType.Fire, 1);
        effectName = "Fire";
        ExecuteSpellCommand();
    }

    private void ExecuteSpellCommand()
    {
        isAttacking = true;
        targetAllButton.SetActive(true);
        StartCoroutine(ProceedSpell());
        ClosePlayerCommand();
        CloseMagicCommand();
        OpenAnnounceHUD("Choose Enemy for Spell target");
    }

    private IEnumerator ProceedSpell()
    {
        yield return new WaitUntil(() => BattleSceneController.Instance.isAttacking == false);

        Destroy(Instantiate(GameManager.Instance.GetVFX("CastVFX"), BattleController.Instance.currentUnitTurn.transform.position, Quaternion.identity), 2);
        GameManager.Instance.PlaySfx("CastSFX");

        yield return new WaitForSeconds(0.64f);

        if (isTargetAllEnemy)
        {
            foreach (var item in BattleController.Instance.enemy)
            {
                if (!item.isDead)
                {
                    Destroy(Instantiate(GameManager.Instance.GetVFX(effectName + "VFX"), item.transform.position, Quaternion.identity), 2);
                    GameManager.Instance.PlaySfx(effectName + "SFX");
                    yield return new WaitForSeconds(0.256f);
                }
            }

            yield return new WaitForSeconds(1);

            foreach (var item in BattleController.Instance.enemy)
            {
                if (!item.isDead)
                {
                    item.Accept(basicSpell);
                }
            }

            yield return new WaitForEndOfFrame();
            isTargetAllEnemy = false;
        }
        else
        {
            Destroy(Instantiate(GameManager.Instance.GetVFX(effectName + "VFX"), BattleController.Instance.singleTarget.transform.position, Quaternion.identity), 2);
            GameManager.Instance.PlaySfx(effectName + "SFX");

            yield return new WaitForSeconds(1);

            BattleController.Instance.singleTarget.Accept(basicSpell);
        }

        yield return new WaitForSeconds(2f);

        BattleController.Instance.EndTurn();
    }

    public void BackToExploreButton()
    {
        StartCoroutine(ProceedEndBattle());        
    }

    private IEnumerator ProceedEndBattle()
    {
        

        yield return new WaitForEndOfFrame();

        LoadScene("ExploreScene");
    }

    
}
