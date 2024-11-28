using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseCharacter> allCharacters = new List<BaseCharacter>();
    public List<BaseCharacter> players = new List<BaseCharacter>();
    public List<BaseCharacter> enemies = new List<BaseCharacter>();

    public BaseCharacter GetClosestTarget(BaseCharacter standardCharacter)
    {
        // 적과 플레이어 중 적절한 타겟 리스트 선택
        List<BaseCharacter> targetList = standardCharacter.isPlayerCharacter ? enemies : players;

        // 리스트가 비어 있다면 null 반환
        if (targetList.Count == 0)
        {
            return null;
        }

        BaseCharacter closestTarget = null; // 가장 가까운 타겟
        float closestDistance = float.MaxValue; // 초기 값은 매우 큰 값으로 설정

        foreach (BaseCharacter potentialTarget in targetList)
        {
            if (!potentialTarget.isLive)
            {
                continue;
            }

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
}
