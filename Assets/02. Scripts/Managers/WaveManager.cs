using GSDatas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;


public class WaveManager : Singleton<WaveManager>
{


    private Coroutine _prepareCoroutine;

    private int _currentWave; // 현재 웨이브 
    private int _endWave; // 마지막 웨이브


    private const float preparationTime = 60f; //대기 시간
    private const float waveCycleTime = 120f;// 한 웨이브 시간
    public event Action<float> OnPreparationTimeChanged;
    private float currentPreparationTime; // 남은 대기 시간
    private bool isPreparing;


    public event Action OnBattleStart; // 전투 시작 확인
    public event Action OnClearWave; // 웨이브 클리어 확인 
    public event Action OnWaveAllClear; // 모든 웨이브 클리어 확인 
    public event Action OnDead; // 스테이지 라이프가 다해 죽었을 때


    //웨이브 정보 저장 - 몬스터 소환 위치 및 정보
    private List<WaveData> _AllWaveData = new List<WaveData>();
    private List<WaveData> _currentWaveData;

    //웨이브 보상 정보 저장
    private List<WaveRewardData> _AllWaveRewardata = new List<WaveRewardData>();
    private List<WaveRewardData> _currentWaveRewardData;
    public int _currentWaveGold;

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

    public List<WaveData> CurrentWaveData
    {
        get { return _currentWaveData; }
    }



    public void Start()
    {

        int stageID = GameManager.Instance.StageID;
        CurrentWave = 1;

        // 해당 스테이지/웨이브에 맞게 재분류
        GetAllWaveData(stageID);
        GetWaveData();


        GetRewardData(stageID);
        GetRewardData();


        OnBattleStart += InventoryManager.Instance.AutoSummonUnits;

        OnClearWave += GetWaveData;
        OnClearWave += GetRewardData;
        OnClearWave += SpawnManager.Instance.SpawnEnemy;

        OnWaveAllClear += StageManager.Instance.GameClear;
        OnDead += StageManager.Instance.GameOver;


        // 게임 시작 시 자동으로 대기 시간 시작
        _prepareCoroutine = StartCoroutine(NextWavePrepare());

    }


    // 웨이브에서 승리했을 때
    public void Victroy()
    {

        if (_prepareCoroutine != null)
        {
            StopCoroutine(_prepareCoroutine);
        }

        ClearWave();

    }

    // 웨이브에서 패했을 때
    public void Lose()
    {
        if (_prepareCoroutine != null)
        {
            StopCoroutine(_prepareCoroutine);
        }

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

    private async void ClearWave()
    {

        IsRunningWave = false;

        if (CurrentWave == _endWave)
        {
            OnWaveAllClear?.Invoke();
            return;

        }

        CurrentWave++;
        //DB에서 불러온 최종 웨이브 값과 비교해서 로직 실행


        //5스테이지 마다 중간 보스?
        if (CurrentWave % 5 == 0)
        {



        }
        else
        {


            foreach (WaveRewardData reward in _currentWaveRewardData)
            {

                if (reward.RewardID == 3001)
                    _currentWaveGold = reward.count;
                    StageManager.Instance.Gold += reward.count;


                Debug.Log($" 추가 재화 : {reward.count} 획득");

            }
        }

        //웨이브 클리어 보상 UI - 중간 보스일때는 3개 선택 창, 일반의 경우 일반 보상 
        UIManager.Instance.OpenUI<UIWaveClear>();
        await Task.Delay(5000);
        UIManager.Instance.CloseUI<UIWaveClear>();

        _prepareCoroutine = StartCoroutine(NextWavePrepare());

    }

    private void StartWave()
    {

        IsRunningWave = true;
        OnBattleStart?.Invoke(); // UI 비활성화 및 자동 유닛 소환

        //유닛 소환이 완료된 후 전투 시작
        StartCoroutine(StartBattleAfterSummon());

    }


    private IEnumerator StartBattleAfterSummon()
    {
        // 유닛 소환이 완료될 때까지 대기
        yield return new WaitForEndOfFrame();

        // 전투 시작
        BattleManager.Instance.BattleSettingAndStart();

        // 전투 시작과 함께 타이머 시작
        currentPreparationTime = waveCycleTime;
        _prepareCoroutine = StartCoroutine(BattleTimer());
    }



    //스테이지 끝나고 준비 시간 
    private IEnumerator NextWavePrepare()
    {
        isPreparing = true;


        // SpawnManager의 초기화가 완료될 때까지 대기
        while (!SpawnManager.Instance.SlotsInitialized)
        {
            yield return null;
        }


        OnClearWave?.Invoke();

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


    private IEnumerator BattleTimer()
    {
        while (currentPreparationTime > 0)
        {
            OnPreparationTimeChanged?.Invoke(currentPreparationTime);
            yield return new WaitForSeconds(1f);
            currentPreparationTime--;
        }

        Debug.Log("타임 오버 / 패배");
        BattleManager.Instance.StartCoroutine(BattleManager.Instance.Lose());
    }


    //최대 스테이지 및 웨이브 데이터 구하기
    private void GetAllWaveData(int stageID)
    {

        _AllWaveData = WaveData.GetList().Where(data => data.ID == stageID).ToList();
        _endWave = _AllWaveData.Max(data => data.wave);

    }

    // wave별로 데이터 가져오기
    private void GetWaveData()
    {
        _currentWaveData = _AllWaveData.Where(data => data.wave == CurrentWave).ToList();
    }


    private void GetRewardData(int stageID)
    {
        _AllWaveRewardata = WaveRewardData.GetList().Where(data => data.ID == stageID).ToList();

    }

    private void GetRewardData()
    {
        _currentWaveRewardData = _AllWaveRewardata.Where(data => data.wave == CurrentWave).ToList();
    }



}
