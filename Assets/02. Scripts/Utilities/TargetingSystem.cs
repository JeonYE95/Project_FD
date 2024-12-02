using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CharacterSearchOptions
{
    public int Number { get; set; }
    public bool RequireOpponent { get; set; } = true;
    public bool ClosestFirst { get; set; } = true;
    public bool IncludeSelf { get; set; } = false;
    public bool UseTargetAsReference { get; set; } = false;

    // 필수 속성은 생성자로 설정
    public CharacterSearchOptions(int number)
    {
        if (number < 1)
        {
            throw new ArgumentException("Number must be at least 1");
        }

        Number = number;
    }
}


public class TargetingSystem
{
    public BattleManager battleManager;

    List<BaseCharacter> players;
    List<BaseCharacter> enemies;

    public TargetingSystem(BattleManager battleManager)
    {
        this.battleManager = battleManager;

        this.players = battleManager.players;
        this.enemies = battleManager.enemies;
    }

    public List<BaseCharacter> GetCharacters(BaseCharacter standardCharacter, CharacterSearchOptions options)
    {
        // 입력 매개변수 널 체크
        if (standardCharacter == null)
        {
            Debug.LogError("standardCharacter is null!");
            return new List<BaseCharacter>();
        }

        // 후보 리스트 생성
        List<BaseCharacter> candidates = GetCandidates(standardCharacter, options.RequireOpponent, options.IncludeSelf);

        // 정렬 또는 랜덤 처리
        if (options.ClosestFirst)
        {
            SortByDistance(candidates, standardCharacter, options.UseTargetAsReference);
        }
        else
        {
            ShuffleList(candidates);
        }

        // 요청된 수만큼 반환
        return GetTopCharacters(candidates, options.Number);
    }


    private List<BaseCharacter> GetCandidates(BaseCharacter standardCharacter, bool requireOpponent, bool includeSelf)
    {
        List<BaseCharacter> candidates = new List<BaseCharacter>();

        if (requireOpponent)
        {
            //상대리스트 가져오기
            List<BaseCharacter> source = standardCharacter.isPlayerCharacter ? enemies : players;

            foreach (BaseCharacter character in source)
            {
                if (character != null && character.isLive)
                {
                    candidates.Add(character);
                }
            }
        }
        else
        {
            //팀원리스트 가져오기
            List<BaseCharacter> source = standardCharacter.isPlayerCharacter ? players : enemies;

            foreach (var character in source)
            {
                //첫번째 조건은 위랑 같지만 두번째는 나 포함 이냐 아니냐 에 따라서 다름
                if (character != null && character.isLive && (includeSelf || character != standardCharacter))
                {
                    candidates.Add(character);
                }
            }
        }

        return candidates;
    }

    private void SortByDistance(List<BaseCharacter> candidates, BaseCharacter standardCharacter, bool useTargetAsReference)
    {
        Transform referenseTransform = useTargetAsReference && standardCharacter.targetCharacter != null
            ? standardCharacter.targetCharacter.transform
            : standardCharacter.transform;

        //정렬 기준 람다 함수로 넣기
        candidates.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(referenseTransform.position, a.transform.position);
            float distanceB = Vector2.Distance(referenseTransform.position, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });
    }

    private void ShuffleList(List<BaseCharacter> candidates)
    {
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            BaseCharacter temp = candidates[i];
            candidates[i] = candidates[randomIndex];
            candidates[randomIndex] = temp;
        }
    }

    private List<BaseCharacter> GetTopCharacters(List<BaseCharacter> candidates, int number)
    {
        List<BaseCharacter> result = new List<BaseCharacter>();

        for (int i = 0; i < number; i++)
        {
            result.Add(candidates[i]);
        }

        return result;
    }

    //가장 가까운 상대편 반환
    public BaseCharacter GetTargetClosestOpponent(BaseCharacter standardCharacter)
    {
        // 상대편 리스트 선택
        //List<BaseCharacter> targetList = GetAliveOpponents(standardCharacter);

        List<BaseCharacter> targetList = GetCandidates(standardCharacter, true, false);

        BaseCharacter closestTarget = null; // 가장 가까운 타겟
        float closestDistance = float.MaxValue; // 초기 값은 매우 큰 값으로 설정

        // 직접 구현(O(n))으로 최적화.
        foreach (BaseCharacter potentialTarget in targetList)
        {
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

    //LINQ 지양하는 쪽으로 코드 리펙토링

/*    public List<BaseCharacter> GetAliveOpponents(BaseCharacter standardCharacter)
    {
        List<BaseCharacter> targetList = standardCharacter.isPlayerCharacter
            ? enemies.Where(target => target.isLive).ToList()
            : players.Where(target => target.isLive).ToList();

        return targetList;
    }

    //기존 타겟이 있으면 타겟 리스트에 추가하는 함수
    void IncludeExistingTarget(BaseCharacter standardCharacter, List<BaseCharacter> targetList, List<BaseCharacter> aliveOpponents)
    {
        if (standardCharacter.targetCharacter != null && standardCharacter.targetCharacter.isLive)
        {
            targetList.Add(standardCharacter.targetCharacter);
            aliveOpponents.Remove(standardCharacter.targetCharacter);
        }
    }

    //타겟과 랜덤 N명 상대편 반환
    public List<BaseCharacter> GetTargetAndRandomOpponents(BaseCharacter standardCharacter, int number)
    {
        if (number < 2)
        {
            throw new ArgumentException("GetRandomOpponentTargets 함수에 넘버 2이상 넣어야됌");
        }

        //상대편 리스트
        List<BaseCharacter> aliveOpponents = GetAliveOpponents(standardCharacter);

        //리턴 할 타겟 리스트
        List<BaseCharacter> targetList = new List<BaseCharacter>();

        IncludeExistingTarget(standardCharacter, targetList, aliveOpponents);
        
        number = Mathf.Min(number - targetList.Count, aliveOpponents.Count);

        // 랜덤 타겟 추가
        for (int i = 0; i < number; i++)
        {
            int random = Random.Range(0, aliveOpponents.Count);
            targetList.Add(aliveOpponents[random]);
            aliveOpponents.RemoveAt(random);
        }

        return targetList;
    }

    //GetTargetAndClosestOpponentsFromTarget 이름 고민중
    //타겟과 타겟에서 가까운 순으로 상대편 N명 반환
    public List<BaseCharacter> GetTargetAndClosestOpponents(BaseCharacter standardCharacter, int number)
    {
        List<BaseCharacter> aliveOpponents = GetAliveOpponents(standardCharacter);

        aliveOpponents = aliveOpponents.OrderBy(target
            => Vector2.Distance(target.transform.position, standardCharacter.transform.position)).ToList();

        List<BaseCharacter > targetList = new List<BaseCharacter>();

        IncludeExistingTarget(standardCharacter, targetList, aliveOpponents);
        number = Mathf.Min(number - targetList.Count, aliveOpponents.Count);

        //TAKE(LINQ) 사용시 / 사용안할시
        targetList.AddRange(aliveOpponents.Take(number));

        *//*for (int i = 0; i < number; i++)
        {
            targetList.Add(aliveOpponents[i]);
        }*//*

        return targetList;
    }*/
}
