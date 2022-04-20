using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BattleSceneController : UIController
{
    public static BattleSceneController Instance { get; private set; }
    public bool isAttacking;
    public CanvasGroup deck, commandHUD, announceHUD;
    public TextMeshProUGUI announceText;
    public GameObject textDamage;

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
        commandHUD.gameObject.SetActive(false);
        announceHUD.gameObject.SetActive(false);
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

    public void ShowTextDamage(int amount, Vector3 pos)
    {
        var temp = Instantiate(textDamage, pos, Quaternion.identity);
        temp.GetComponent<TextMeshPro>().text = amount.ToString();
        temp.transform.DOJump(temp.transform.position, 1, 1, 0.5f);
        Destroy(temp, 1.5f);
    }
}
