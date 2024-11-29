using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseCharacter> allCharacters = new List<BaseCharacter>();
    public List<BaseCharacter> players = new List<BaseCharacter>();
    public List<BaseCharacter> enemies = new List<BaseCharacter>();

    //살아있는 캐릭터와 죽어있는 캐릭터 용 리스트 필요??
    //어느정도는 필요한게 적들 다 죽거나 플레이어 다 죽으면 웨이브(배틀) 이 끝나야 함
    private int totalCharacters => players.Count + enemies.Count;  // 전체 캐릭터 수 자동집계


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (BaseCharacter character in players)
            {
                allCharacters.Add(character);
            }

            foreach (BaseCharacter character in enemies)
            {
                allCharacters.Add(character);
            }

            foreach (BaseCharacter character in allCharacters)
            {
                character.ActiveCharacter();
            }
        }
    }

    public void RegisterCharacter(BaseCharacter character)
    {
        if (character.isPlayerCharacter)
        {
            players.Add(character);
        }
        else
        {
            enemies.Add(character);
        }
    }

    
    public BaseCharacter GetClosestOpponentTarget(BaseCharacter standardCharacter)
    {
        // 적과 플레이어 중 적절한 타겟 리스트 선택
        List<BaseCharacter> targetList = standardCharacter.isPlayerCharacter ? enemies : players;
        targetList = targetList.Where(target => target.isLive).ToList();

        if (targetList.Count == 0)
        {
            return new BaseCharacter();
        }

        BaseCharacter closestTarget = null; // 가장 가까운 타겟
        float closestDistance = float.MaxValue; // 초기 값은 매우 큰 값으로 설정

        foreach (BaseCharacter potentialTarget in targetList)
        {
            //위에서 체크함
            /*if (!potentialTarget.isLive)
            {
                continue;
            }*/

            // 현재 타겟과의 거리 계산
            float currentDistance = Vector2.Distance(standardCharacter.transform.position, potentialTarget.transform.position);

            // 더 짧은 거리라면 가장 가까운 타겟 갱신
            if (currentDistance < closestDistance)
            {
                closestTarget = potentialTarget;
                closestDistance = currentDistance;
            }
        }

        return closestTarget;
    }

    public List<BaseCharacter> GetRandomOpponentTargets(BaseCharacter standardCharacter, int number)
    {
        if (number < 2)
        {
            throw new ArgumentException("GetRandomOpponentTargets 함수에 넘버 2이상 넣어야됌");
        }

        //상대편 리스트
        List<BaseCharacter> aliveOpponents = standardCharacter.isPlayerCharacter ? enemies : players;
        //살아있는 캐릭터만
        aliveOpponents = aliveOpponents.Where(target => target.isLive).ToList();
        //리턴 할 타겟 리스트
        List<BaseCharacter> targetList = new List<BaseCharacter>();

        if (aliveOpponents.Count == 0)
        {
            return new List<BaseCharacter>();
        }

        //기존에 타겟이 있었으면 무조건 그 타겟은 타겟 리스트에 포함
        if (standardCharacter.targetCharacter != null && standardCharacter.targetCharacter.isLive)
        {
            targetList.Add(standardCharacter.targetCharacter);
            aliveOpponents.Remove(standardCharacter.targetCharacter);
            number--;
        }

        // 요청한 타겟 수 제한
        number = Mathf.Min(number, aliveOpponents.Count);

        // 랜덤 타겟 추가
        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(0, aliveOpponents.Count);
            targetList.Add(aliveOpponents[random]);
            aliveOpponents.RemoveAt(random);
        }

        return targetList;
    }
}
