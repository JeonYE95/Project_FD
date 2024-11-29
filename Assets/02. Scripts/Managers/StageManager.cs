using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class StageManager : Singleton<StageManager>
{


    [Header("스테이지 라이프 관리")]
    private int stageHealth = 3;


    [Header("웨이브 관리")]
    private int waveCount;
    private const float preparationTime = 30f; // 대기 시간
    private static readonly WaitForSeconds preparation = new WaitForSeconds(preparationTime); // 웨이브 끝난 후 대기 시간 코루틴 

    public int currentWave { get; private set; }
    public bool isRunningWave { get; private set; }

  
    public event Action<int> onClearWave; // 웨이브 클리어 확인 
    public event Action onWaveAllClear; // 모든 웨이브 클리어 확인 - DB에서 불러와 연동해야.

    private Coroutine preparationCoroutine;
 



    public void Init()
    {
        waveCount = 1;
        currentWave = 1;

        onWaveAllClear += GameClear;

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
        // 체력이 다 깎였을때 - 게임 오버 UI 불러오기


    }

    public void GameClear()
    {
        // 게임 클리어 시 UI 불러오기


    }

  
    public void WaveStartNow()
    {

        //버튼 연동 필요 - StopCoroutine 진행, StartWave로 곧바로 넘어감

        StopCoroutine(NextWavePrepare());

        StartWave();
    
    }

    private void ClearWave()
    {


        currentWave++;
    
    }

    private void StartWave()
    {

   
        isRunningWave = true;

        // 몬스터, 플레이어 소환
        //

    }




    private void EndWave()
    {
        isRunningWave = false;
        onClearWave?.Invoke(currentWave);


        //DB에서 불러온 최종 스테이지 값과 비교해서 로직 실행
        /*
          
        if(currentWave == DB 스테이지 최종 웨이브 )
        { 
             OnWaveAllClear?.Invoke();
             return;
        }
         
       */


        // 보상 UI 불러오기

    }


    // 스테이지 끝나고 준비시간
    private IEnumerator NextWavePrepare()
    {
     
        yield return preparation;
        StartWave();

    }



}