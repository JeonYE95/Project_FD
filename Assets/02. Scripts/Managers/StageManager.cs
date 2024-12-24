using System;
using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;

public class StageManager : Singleton<StageManager>
{

    [Header("스테이지 관리")]
    private int _stageHealth = 3;

    [SerializeField]
    private int _StageId;

    public int StageHealth
    {
        get { return _stageHealth; }
        set { _stageHealth = value; }
    }


    [Header("스테이지 내 재화 관리")]
    private int _gold = 15;


    public int Gold
    {

        get
        { return _gold; }
        set
        {
            _gold = value;

            if (_gold < 0)
                _gold = 0;

        }

    }

    protected override void Awake()
    {
        base.Awake();
        _StageId = GameManager.Instance.StageID;
   
    }


    public void StopGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void GameOver()
    {


        StageHealth -= 1;

        // 체력이 다 깎였을때 - 게임 오버 UI 불러오기
        if (StageHealth <= 0)
        {
            UIManager.Instance.OpenUI<UIGameOver>();
            return;
        }


        //재시작 

    }

    public void GameClear()
    {
        // 게임 클리어 시 UI 불러오기 - 보상 받음
        UIManager.Instance.OpenUI<UIStageClear>();


        // 보상 획득
        GameManager.Instance.ClearReward(_StageId);


        _stageHealth = 3;

        Debug.Log("스테이지 클리어");

    }


 

}