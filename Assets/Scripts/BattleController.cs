using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleController : MonoBehaviour
{
    private UnitBase player;
    private UnitBase enemy;

    public BattleState state;
    public Transform[] enemyStations;
    public Transform[] playerStations;
    public GameObject enemyPrefab;
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        SetUpBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetUpBattle()
    {
        GameObject _player = Instantiate(playerPrefab, playerStations[0].position, Quaternion.identity);
        player = _player.GetComponent<UnitBase>();

        GameObject _enemy = Instantiate(enemyPrefab, enemyStations[0].position, Quaternion.identity);
        enemy = _enemy.GetComponent<UnitBase>();
    }
}
