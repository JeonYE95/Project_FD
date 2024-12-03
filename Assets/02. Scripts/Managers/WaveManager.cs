using System;
using System.Collections;
using UnityEngine;


public class WaveManager : Singleton<WaveManager>
{

    private int _waveCount;
    private const float preparationTime = 30f; //대기 시간
    public event Action<float> OnPreparationTimeChanged;
    private float currentPreparationTime; // 남은 대기 시간
    private bool isPreparing;

    public event Action<int> OnClearWave; // 웨이브 클리어 확인 
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
        StopCoroutine(NextWavePrepare());

        isPreparing = false;

        StartWave();

    }

    private void ClearWave()
    {

        IsRunningWave = false;

        OnClearWave?.Invoke(CurrentWave);


        //DB에서 불러온 최종 스테이지 값과 비교해서 로직 실행
        /*
          
        if(currentWave == DB 스테이지 최종 웨이브 )
        { 
            OnWaveAllClear?.Invoke();
            return;
          
        }
         
       */

        //웨이브 클리어 보상 UI


        //StageManager.Instance.Gold += DB에서 정해진 값 불러오기;
        //StagetManager.Instance.Diamond += DB에서 정해진 값 불러오기;


        CurrentWave++;
        StartCoroutine(NextWavePrepare());

    }

    private void StartWave()
    {

        IsRunningWave = true;

        //플레이어, 몬스터 소환


    }


    //스테이지 끝나고 준비 시간 
    private IEnumerator NextWavePrepare()
    {
        isPreparing = true;
        currentPreparationTime = preparationTime;

        while (currentPreparationTime > 0)
        {
            OnPreparationTimeChanged?.Invoke(currentPreparationTime);
            yield return new WaitForSeconds(1f);
            currentPreparationTime--;
        }

        isPreparing = false;
        StartWave();

    }


    public string GetCurrentPreparationTimeText()
    {
        int seconds = Mathf.FloorToInt(currentPreparationTime % 60);
        return $"{seconds:00}";
    }

}
