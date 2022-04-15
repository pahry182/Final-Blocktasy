using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    public BattleState state;
    public UnitBase[] player;
    public UnitBase[] enemy;
    public Transform[] enemyStations;
    public Transform[] playerStations;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;
    [Header("HUD")]
    public TextMeshProUGUI[] textEnemyNames;
    public TextMeshProUGUI[] textEnemyCounts;
    public TextMeshProUGUI[] textPlayerNames;
    public TextMeshProUGUI[] textPlayerHps;
    public TextMeshProUGUI[] textPlayerMaxHps;
    public TextMeshProUGUI[] textPlayerMps;

    public Image[] playerGauges;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurn();
    }

    private void SetUpBattle()
    {
        for (int i = 0; i < player.Length; i++)
        {
            GameObject _player = Instantiate(playerPrefab, playerStations[i].position, Quaternion.identity);
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

        foreach (var item in filteredNames)
        {
            if (item == null) continue;
            print(item);
        }
    }

    private void UpdateTurn()
    {
        if (state == BattleState.PLAYERTURN) return;

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
}
