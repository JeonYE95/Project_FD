using System;
using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.SceneManagement;

public class StageManager : Singleton<StageManager>
{

    // 스테이지 데이터 저장
    private List<StageData> _currentStageData = new List<StageData>();

    [Header("스테이지 관리")]
    private int _stageHealth = 3;
    private int _StageId;

    public int StageId
    { 
        get { return _StageId;}
    }

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

        //현재는 StageID 임의로 주입 - 
        _StageId = 101;
        GetStatgeData(_StageId);


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

            return;

        }


        //재시작 

    }

    public void GameClear()
    {
        // 게임 클리어 시 UI 불러오기 - 보상 받음


        // 보상 획득
        if (_currentStageData == null)
        {
            GetStatgeData(_StageId);
        }

        foreach (StageData reward in _currentStageData)
        {

            RewardData rewardInfo = RewardDataManager.Instance.GetUnitData(reward.RewardID);

            Debug.Log($" 스테이지 클리어 보상 : {rewardInfo.name} : {reward.count} 획득");

            GameManager.Instance.AddItem(reward.RewardID, reward.count);

        }


        _stageHealth = 3;

        Debug.Log("스테이지 클리어");

    }


    //스테이지 보상 데이터 가져오기 
    private void GetStatgeData(int stageID)
    {

        _currentStageData = StageData.GetList().Where(data => data.ID == stageID).ToList();
    

    }



  

}