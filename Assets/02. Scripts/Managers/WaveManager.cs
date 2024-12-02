using System;
using System.Collections;
using UnityEngine;


public class WaveManager : Singleton<WaveManager>
{

    private int _waveCount;
    private const float preparationTime = 30f; // 대기 시간
    private static readonly WaitForSeconds preparation = new WaitForSeconds(preparationTime); // 웨이브 끝난 후 대기 시간 코루틴 


    public event Action<int> OnClearWave; // 웨이브 클리어 확인 
    public event Action OnWaveAllClear; // 모든 웨이브 클리어 확인 - DB에서 불러와 연동해야.
    public event Action OnDead; // 스테이지 라이프가 다해 죽었을 때


    public int CurrentWave
    {
        get { return _waveCount; }
        private set { _waveCount = value; }

    }

    public bool IsRunningWave { get; private set; }

    public int MonsterRemainCount { get; private set; }
    public int PlayerRemainCount { get; private set; }


    public void Start()
    {

        CurrentWave = 1;

        OnWaveAllClear += StageManager.Instance.GameClear;
        OnDead += StageManager.Instance.GameOver;

    }



    private void Victroy()
    {

        MonsterRemainCount--;

        if (MonsterRemainCount == 0)
        {
            ClearWave();

        }


    }

    private void Lose()
    {
        PlayerRemainCount--;

        if (PlayerRemainCount == 0)
        {
            StageManager.Instance.GameOver();

        }


    }

    public void WaveStartNow()
    {

        //버튼 연동 필요 - StopCoroutine 진행, StartWave로 곧바로 넘어감

        StopCoroutine(NextWavePrepare());

        StartWave();

    }

    public void ClearWave()
    {

        IsRunningWave = false;

        CurrentWave++;


    }

    private void StartWave()
    {


        IsRunningWave = true;


        PlayerRemainCount = BattleManager.Instance.players.Count;
        MonsterRemainCount = BattleManager.Instance.enemies.Count;

        //플레이어, 몬스터 소환



    }


    private void EndWave()
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


        //StageManager.Instance.Gold += ;
        //StagetManager.Instance.Diamond += ;

        StartCoroutine(NextWavePrepare());

    }

    //스테이지 끝나고 준비 시간 
    private IEnumerator NextWavePrepare()
    {

        yield return preparation;
        StartWave();

    }


}
