using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public static BattleController Instance { get; private set; }

    public BattleState State { get; internal set; }
    public UnitBase[] player;
    public UnitBase[] enemy;
    public UnitBase currentUnitTurn, singleTarget;
    public Transform[] enemyStations;
    public Transform[] playerStations;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    private bool isSetupCompleted;
    [Header("HUD")]
    public TextMeshProUGUI[] textEnemyNames;
    public TextMeshProUGUI[] textEnemyCounts;
    public TextMeshProUGUI[] textPlayerNames;
    public TextMeshProUGUI[] textPlayerHps;
    public TextMeshProUGUI[] textPlayerMaxHps;
    public TextMeshProUGUI[] textPlayerMps;

    public Image[] playerGauges;

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

    // Start is called before the first frame update
    void Start()
    {
        State = BattleState.START;
        StartCoroutine(SetUpBattle());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurn();
        CheckTurn();
    }

    private IEnumerator SetUpBattle()
    {
        for (int i = 0; i < player.Length; i++)
        {
            GameObject _player = Instantiate(GameManager.Instance.playerCharacters[i], playerStations[i].position, Quaternion.identity);
            player[i] = _player.GetComponent<UnitBase>();
            textPlayerNames[i].text = player[i].unitName;
            textPlayerHps[i].text = player[i].currentHp.ToString();
            textPlayerMps[i].text = player[i].currentMp.ToString();
            textPlayerMaxHps[i].text = player[i].maxHp.ToString();
            playerGauges[i].fillAmount = player[i].turnGauge / 1000;
        }

        for (int i = 0; i < enemy.Length; i++)
        {
            GameObject _enemy = Instantiate(enemyPrefab, enemyStations[i].position, Quaternion.identity);

            enemy[i] = _enemy.GetComponent<UnitBase>();
        }

        SetupEnemyNameList();

        yield return new WaitForEndOfFrame();

        isSetupCompleted = true;

    }

    private void SetupEnemyNameList()
    {
        string[] names = new string[8];
        List<string> filteredNames = new List<string>();

        for (int i = 0; i < enemy.Length; i++)
        {
            names[i] = enemy[i].unitName;
            if (!filteredNames.Exists(element => element == names[i]))
            {
                filteredNames.Add(names[i]);
            }
        }

        filteredNames.Sort();

        for (int i = 0; i < textEnemyNames.Length; i++)
        {
            textEnemyNames[i].gameObject.SetActive(false);
            textEnemyCounts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < filteredNames.Count; i++)
        {
            textEnemyNames[i].gameObject.SetActive(true);
            textEnemyCounts[i].gameObject.SetActive(true);
            textEnemyNames[i].text = filteredNames[i];
            string[] singleName = Array.FindAll(names, element => element == filteredNames[i]);
            textEnemyCounts[i].text = singleName.Length.ToString();
        }

        //foreach (var item in filteredNames)
        //{
        //    if (item == null) continue;
        //    print(item);
        //}
    }

    private void UpdateTurn()
    {
        if (!isSetupCompleted) return;
        if ((State == BattleState.PLAYERTURN) || (State == BattleState.ENEMYTURN)) return;

        foreach (var item in enemy)
        {
            if (item.turnGauge > 1000) return;
            item.turnGauge += item.attSpeed + Time.deltaTime;
        }
        
        foreach (var item in player)
        {
            if (item.turnGauge > 1000) return;
            item.turnGauge += item.attSpeed + Time.deltaTime;
        }

        for (int i = 0; i < playerGauges.Length; i++)
        {
            playerGauges[i].fillAmount = player[i].turnGauge / 1000;
        }
    }

    private void CheckTurn()
    {
        if (!isSetupCompleted) return;
        if ((State == BattleState.PLAYERTURN) || (State == BattleState.ENEMYTURN)) return;

        foreach (var item in enemy)
        {
            if (item.turnGauge >= 1000)
            {
                currentUnitTurn = item;
                State = BattleState.ENEMYTURN;
            }
        }

        foreach (var item in player)
        {
            if (item.turnGauge >= 1000)
            {
                currentUnitTurn = item;
                State = BattleState.PLAYERTURN;
                int index = Array.IndexOf(player, item);
                playerGauges[index].GetComponent<Image>().color = Color.yellow;
                BattleSceneController.Instance.OpenPlayerCommand();
            }
        }
    }

    public void ApplyDamage(UnitBase unit, int amount)
    {
        StartCoroutine(ApplyDamageC(unit, amount));
    }

    private IEnumerator ApplyDamageC(UnitBase unit, int amount)
    {
        yield return new WaitForSeconds(1.5f);

        unit.currentHp -= amount;

        if (unit.currentHp < 1)
        {
            unit.isDead = true;
            StartCoroutine(unit.StartDie());
        }
    }

    public void EndTurn()
    {
        currentUnitTurn.turnGauge = 0;
        int index = Array.IndexOf(player, currentUnitTurn);
        playerGauges[index].GetComponent<Image>().color = Color.white;
        State = BattleState.START;
    }
}
