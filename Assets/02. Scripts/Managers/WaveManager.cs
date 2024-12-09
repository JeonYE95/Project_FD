using GSDatas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class WaveManager : Singleton<WaveManager>
{


    private Coroutine _prepareCoroutine;

    private int _currentWave; // 현재 웨이브 
    private int _endWave; // 마지막 웨이브


    private const float preparationTime = 30f; //대기 시간
    public event Action<float> OnPreparationTimeChanged;
    private float currentPreparationTime; // 남은 대기 시간
    private bool isPreparing;


    public event Action OnBattleStart; // 전투 시작 확인
    public event Action OnClearWave; // 웨이브 클리어 확인 
    public event Action OnWaveAllClear; // 모든 웨이브 클리어 확인 
    public event Action OnDead; // 스테이지 라이프가 다해 죽었을 때


    //웨이브 정보 저장 - 몬스터 소환 위치 및 정보
    private List<WaveData> _AllStageWaveData = new List<WaveData>();
    private List<WaveData> _currentStageWaveData; 



    public int CurrentWave
    {
        get { return _currentWave; }
        private set { _currentWave = value; }

    }

    public int EndWave
    {
        get { return _endWave; }
        private set { _endWave = value; }

    }

    public bool IsRunningWave { get; private set; }

    public List<WaveData> CurrentStageWaveData
    {
        get { return _currentStageWaveData; }
    }


    public void Start()
    {

        //임시로 스테이지 ID 넣음  -> 나중에 스테이지 진입시 정보를 받아와야 함.

        int stageID = 101;
        CurrentWave = 1;

        // 해당 스테이지/웨이브에 맞게 재분류
        GetStageData(stageID);
        GetWaveData();




        OnClearWave += GetWaveData;
        OnClearWave += SpawnManager.Instance.SpawnEnemy; 


        OnWaveAllClear += StageManager.Instance.GameClear;
        OnDead += StageManager.Instance.GameOver;


        // 게임 시작 시 자동으로 대기 시간 시작
        _prepareCoroutine = StartCoroutine(NextWavePrepare());

    }


    // 웨이브에서 승리했을 때
    public void Victroy()
    {
        ClearWave();

    }

    // 웨이브에서 패했을 때
    public void Lose()
    {
        IsRunningWave = false;
        OnDead?.Invoke();
        StartCoroutine(NextWavePrepare());

    }

    public void WaveStartNow()
    {
        if (_prepareCoroutine != null)
        {

            StopCoroutine(_prepareCoroutine);
            _prepareCoroutine = null;

        }

        StartWave();

    }

    private void ClearWave()
    {

        IsRunningWave = false;

        CurrentWave++;
        //DB에서 불러온 최종 웨이브 값과 비교해서 로직 실행

        if (CurrentWave == _endWave)
        {
            OnWaveAllClear?.Invoke();
            return;

        }


        //웨이브 클리어 보상 UI - 중간 보스일때는 3개 선택 창, 일반의 경우 일반 보상 
        /*
      
        //5스테이지 마다 중간 보스?
        f (CurrentWave % 5 == 0)
        {
        


        }
        else 
        {
        

         //StageManager.Instance.Gold += DB에서 정해진 값 불러오기;
         //StagetManager.Instance.Diamond += DB에서 정해진 값 불러오기;


        }

        */

        _prepareCoroutine = StartCoroutine(NextWavePrepare());

    }

    private void StartWave()
    {

        IsRunningWave = true;
        OnBattleStart?.Invoke(); // UI 비활성화

        //전투 시작
        //BattleManager.Instance.BattleSetingAndStart(); 


        //몬스터 소환


    }


    //스테이지 끝나고 준비 시간 
    private IEnumerator NextWavePrepare()
    {

        OnClearWave?.Invoke();
        isPreparing = true;

        //UI 활성화 필요
        //

        currentPreparationTime = preparationTime;

        while (currentPreparationTime > 0)
        {
            OnPreparationTimeChanged?.Invoke(currentPreparationTime);
            yield return new WaitForSeconds(1f);
            currentPreparationTime--;
        }

        isPreparing = false;
        Debug.Log("게임 시작");
        StartWave();

    }


    public string GetCurrentPreparationTimeText()
    {
        int seconds = Mathf.FloorToInt(currentPreparationTime % 60);
        return $"{seconds:00}";
    }


    //최대 스테이지 및 웨이브 데이터 구하기
    public void GetStageData(int stageID)
    {

        _AllStageWaveData = WaveData.GetList().Where(data => data.ID == stageID).ToList();
        _endWave = _AllStageWaveData.Max(data => data.wave);

    }

    // wave별로 데이터 가져오기
    public void GetWaveData()
    {
        _currentStageWaveData =  _AllStageWaveData.Where(data => data.wave == CurrentWave).ToList();
    }



}
