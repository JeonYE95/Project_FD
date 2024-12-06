using System;
using System.Collections;
using UnityEngine;


public class WaveManager : Singleton<WaveManager>
{


    private Coroutine _prepareCoroutine;

    private int _waveCount;
    private const float preparationTime = 30f; //대기 시간
    public event Action<float> OnPreparationTimeChanged;
    private float currentPreparationTime; // 남은 대기 시간
    private bool isPreparing;


    public event Action OnBattleStart; // 전투 시작 확인
    public event Action OnClearWave; // 웨이브 클리어 확인 
    public event Action OnWaveAllClear; // 모든 웨이브 클리어 확인 - DB에서 불러와 연동해야.
    public event Action OnDead; // 스테이지 라이프가 다해 죽었을 때


    public int CurrentWave
    {
        get { return _waveCount; }
        private set { _waveCount = value; }

    }

    public bool IsRunningWave { get; private set; }


    public void Start()
    {

        CurrentWave = 1;

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
        //DB에서 불러온 최종 스테이지 값과 비교해서 로직 실행
        /*
          
        if(currentWave == DB 스테이지 최종 웨이브 )
        { 
            OnWaveAllClear?.Invoke();
            return;
          
        }
         
       */

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
        //BattleManager.Instance.  전투시작


        //몬스터 소환


    }


    //스테이지 끝나고 준비 시간 
    private IEnumerator NextWavePrepare()
    {

        OnClearWave?.Invoke();
        isPreparing = true;

        //UI 활성화 
       

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

}
