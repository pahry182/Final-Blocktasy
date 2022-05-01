using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ExploreSceneController : UIController
{
    public PlayerExploreController _pec;
    private Vector2 initialPos;

    public CanvasGroup menu;

    private void Awake()
    {

    }

    private void Start()
    {
        GameManager.Instance.PlayBgm("MainMenu");
        SetupExplore();
        StartCoroutine(CheckEncounter());
    }

    private void SetupExplore()
    {
        Vector2 pos = GameManager.Instance.data.GetStageProgress();
        _pec.transform.position = pos;
    }

    private void SaveExplore()
    {
        GameManager.Instance.data.SetStageProgress(_pec.transform.position);
    }

    private IEnumerator CheckEncounter()
    {
        while (true)
        {
            initialPos = _pec.transform.position;

            yield return new WaitForSeconds(1);

            if (Vector2.Distance(initialPos, _pec.transform.position) >= 1)
            {
                int rand = Random.Range(0, 10);
                if (rand <= 1)
                {
                    SaveExplore();
                    yield return new WaitForEndOfFrame();
                    _pec.StopAllCoroutines();
                    LoadScene("BattleScene");
                }
            }
        }  
    }
}
