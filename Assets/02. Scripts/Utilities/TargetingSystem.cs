using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class UnitSearchOptions
{
    public int Number { get; set; }
    public bool RequireOpponent { get; set; } = true;
    public bool ClosestFirst { get; set; } = true;
    public bool IncludeSelf { get; set; } = false;
    public bool UseTargetAsReference { get; set; } = false;

    // 필수 속성은 생성자로 설정
    public UnitSearchOptions(int number)
    {
        Number = number;
    }
}


public class TargetingSystem
{
    public BattleManager battleManager;

    List<BaseUnit> players;
    List<BaseUnit> enemies;

    public TargetingSystem(BattleManager battleManager)
    {
        this.battleManager = battleManager;

        this.players = battleManager.players;
        this.enemies = battleManager.enemies;
    }

    public List<BaseUnit> GetUnits(BaseUnit standardUnit, UnitSearchOptions options)
    {
        // 입력 매개변수 널 체크
        if (standardUnit == null)
        {
            Debug.LogError("standardUnit is null!");
            return new List<BaseUnit>();
        }

        // 후보 리스트 생성
        List<BaseUnit> candidates = GetCandidates(standardUnit, options.RequireOpponent, options.IncludeSelf);

        // 정렬 또는 랜덤 처리
        if (options.ClosestFirst)
        {
            SortByDistance(candidates, standardUnit, options.UseTargetAsReference);
        }
        else
        {
            ShuffleList(candidates);
        }

        // 요청된 수만큼 반환
        return GetTopUnits(candidates, options.Number);
    }


    private List<BaseUnit> GetCandidates(BaseUnit standardUnit, bool requireOpponent, bool includeSelf)
    {
        List<BaseUnit> candidates = new List<BaseUnit>();

        if (requireOpponent)
        {
            //상대리스트 가져오기
            List<BaseUnit> source = standardUnit.isPlayerUnit ? enemies : players;

            foreach (BaseUnit unit in source)
            {
                if (unit != null && unit.isLive)
                {
                    candidates.Add(unit);
                }
            }
        }
        else
        {
            //팀원리스트 가져오기
            List<BaseUnit> source = standardUnit.isPlayerUnit ? players : enemies;

            foreach (var unit in source)
            {
                //첫번째 조건은 위랑 같지만 두번째는 나 포함 이냐 아니냐 에 따라서 다름
                if (unit != null && unit.isLive && (includeSelf || unit != standardUnit))
                {
                    candidates.Add(unit);
                }
            }
        }

        return candidates;
    }

    private void SortByDistance(List<BaseUnit> candidates, BaseUnit standardUnit, bool useTargetAsReference)
    {
        Transform referenseTransform = useTargetAsReference && standardUnit.targetUnit != null
            ? standardUnit.targetUnit.transform
            : standardUnit.transform;

        //정렬 기준 람다 함수로 넣기
        candidates.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(referenseTransform.position, a.transform.position);
            float distanceB = Vector2.Distance(referenseTransform.position, b.transform.position);
            return distanceA.CompareTo(distanceB);
        });
    }

    private void ShuffleList(List<BaseUnit> candidates)
    {
        for (int i = candidates.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            BaseUnit temp = candidates[i];
            candidates[i] = candidates[randomIndex];
            candidates[randomIndex] = temp;
        }
    }

    private List<BaseUnit> GetTopUnits(List<BaseUnit> candidates, int number)
    {
        List<BaseUnit> result = new List<BaseUnit>();

        for (int i = 0; i < number; i++)
        {
            result.Add(candidates[i]);
        }

        return result;
    }

    //가장 가까운 상대편 반환
    public BaseUnit GetTargetClosestOpponent(BaseUnit standardUnit)
    {
        // 상대편 리스트 선택
        //List<BaseCharacter> targetList = GetAliveOpponents(standardCharacter);

        List<BaseUnit> targetList = GetCandidates(standardUnit, true, false);

        BaseUnit closestTarget = null; // 가장 가까운 타겟
        float closestDistance = float.MaxValue; // 초기 값은 매우 큰 값으로 설정

        // 직접 구현(O(n))으로 최적화.
        foreach (BaseUnit potentialTarget in targetList)
        {
            // 현재 타겟과의 거리 계산
            float currentDistance = Vector2.Distance(standardUnit.transform.position, potentialTarget.transform.position);

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
