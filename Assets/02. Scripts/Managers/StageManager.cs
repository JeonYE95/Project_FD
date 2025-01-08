using System;
using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.InteropServices;


public class StageManager : Singleton<StageManager>
{

    [Header("스테이지 관리")]
    private int _stageHealth = 3;

    [SerializeField]
    private int _stageID;

    public int StageHealth
    {
        get { return _stageHealth; }
        set { _stageHealth = value; }
    }

    // private List<StageData> _currentStageData;
    // 스테이지 정보 저장 

    private List<StageData> _currentStageData;
    public List<StageData> CurrentStageData
    {
        get { return _currentStageData; }
        set { _currentStageData = value; }
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
        _stageID = GameManager.Instance.StageID;
   
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
        // 보상 불러오기
        _currentStageData = GetCurrentStageData();

        // 게임 클리어 시 UI 불러오기 - 보상 받음
        UIManager.Instance.OpenUI<UIStageClear>();

        // 보상 획득
        GameManager.Instance.ClearReward(_stageID);

        //스테이지 퀘스트 업데이트
        QuestManager.Instance.UpdateStageQuests(_stageID);

        _stageHealth = 3;

        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlaySFX("IngameUI/StageClear");

        GameManager.Instance.playerData.StageClearData[_stageID] = Defines.StageClearState.Clear;
        GameManager.Instance.playerData.StageClearData[_stageID + 1] = Defines.StageClearState.Unlock;

        Debug.Log("스테이지 클리어");

    }

    private List<StageData> GetCurrentStageData()
    {
        return StageData.GetList().Where(data => data.ID == _stageID).ToList();
    }
}
