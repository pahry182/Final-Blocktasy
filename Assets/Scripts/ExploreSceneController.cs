using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ExploreSceneController : UIController
{
    private PlayerExploreController _pec;
    private Vector2 initialPos;

    public CanvasGroup menu;

    private void Awake()
    {
        _pec = GameObject.FindObjectOfType<PlayerExploreController>();
    }

    private void Start()
    {
        StartCoroutine(CheckEncounter());
    }

    private IEnumerator CheckEncounter()
    {
        while (true)
        {
            initialPos = _pec.transform.position;

            yield return new WaitForSeconds(1);

            if (Vector2.Distance(initialPos, _pec.transform.position) >= 1)
            {
                StartEncounter();
            }
        }  
    }

    private void StartEncounter()
    {
        int rand = Random.Range(0, 10);
        if (rand <= 3)
        {
            LoadScene("BattleScene");
        }
        
    }
}
