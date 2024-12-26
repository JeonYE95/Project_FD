using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UnitSearchOptions
{
    public int Number { get; set; } = 1; // 기본값 : 한명
    public TargetGroup Group { get; set; } = TargetGroup.Enemy; // 기본값: 적군
    public TargetPriority Priority { get; set; } = TargetPriority.Closest; // 기본값: 가까운 순서
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
        List<BaseUnit> candidates = new List<BaseUnit>();

        // 그룹에 따른 후보군 설정
        switch (options.Group)
        {
            case TargetGroup.Enemy:
                candidates = (standardUnit.isPlayerUnit ? enemies : players)
                    .Where(unit => unit != standardUnit)
                    .ToList();
                break;

            case TargetGroup.AllEnemy:
                candidates = (standardUnit.isPlayerUnit ? enemies : players).ToList();
                break;

            case TargetGroup.Ally:
                candidates = (standardUnit.isPlayerUnit ? players : enemies)
                    .Where(unit => unit != standardUnit)
                    .ToList();
                break;

            case TargetGroup.AllAlly:
                candidates = (standardUnit.isPlayerUnit ? players : enemies).ToList();
                break;

            case TargetGroup.Self:
                candidates.Add(standardUnit);
                return candidates;

            case TargetGroup.Target:
                candidates.Add(standardUnit.targetUnit);
                return candidates;

            default:
                Debug.LogWarning($"Unhandled TargetGroup: {options.Group}");
                return new List<BaseUnit>();
        }

        // 생존한 유닛만 포함
        candidates = candidates.Where(unit => unit.isLive).ToList();

        // 우선순위에 따른 정렬
        if (options.Priority == TargetPriority.Closest)
        {
            SortByDistance(candidates, standardUnit, false, false); // 가까운 순
        }
        else if (options.Priority == TargetPriority.Farthest)
        {
            SortByDistance(candidates, standardUnit, false, true); // 먼 순
        }
        else if (options.Priority == TargetPriority.LowestHP)
        {
            SortUnitsByHealthRatio(candidates); // 체력 적은 순 (주로 힐 스킬에 사용)
        }
        else if (options.Priority == TargetPriority.Random)
        {
            ShuffleList(candidates); // 랜덤
        }

        // 타겟 수 제한
        if (options.Number > 0 && options.Number < candidates.Count)
        {
            return GetTopUnits(candidates, options.Number);
        }

        return candidates;
    }

    private void SortUnitsByHealthRatio(List<BaseUnit> candidates, bool isDescending = false)
    {
        candidates.Sort((a, b) =>
        {
            float HPA = (float)a.healthSystem.currentHP / (float)a.healthSystem.MaxHP;
            float HPB = (float)b.healthSystem.currentHP / (float)b.healthSystem.MaxHP;

            return isDescending ? HPB.CompareTo(HPA) : HPA.CompareTo(HPB);
        });
    }

    private void SortByDistance(List<BaseUnit> candidates, BaseUnit standardUnit, bool useTargetAsReference, bool isDescending = false)
    {
        Transform referenceTransform = useTargetAsReference && standardUnit.targetUnit != null
            ? standardUnit.targetUnit.transform
            : standardUnit.transform;

        candidates.Sort((a, b) =>
        {
            float distanceA = Vector2.Distance(referenceTransform.position, a.transform.position);
            float distanceB = Vector2.Distance(referenceTransform.position, b.transform.position);

            // isDescending에 따라 정렬 방향 결정
            return isDescending ? distanceB.CompareTo(distanceA) : distanceA.CompareTo(distanceB);
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

        for (int i = 0; i < number && i < candidates.Count; i++) // 초과 방지
        {
            result.Add(candidates[i]);
        }

        return result;
    }

    public BaseUnit GetTargetClosestOpponent(BaseUnit standardUnit)
    {
        var options = new UnitSearchOptions()
        {
            Group = TargetGroup.Enemy,
            Priority = TargetPriority.Closest
        };

        List<BaseUnit> closestEnemies = GetUnits(standardUnit, options);
        return closestEnemies.FirstOrDefault(); // 가장 가까운 적 반환
    }

    public BaseUnit GetTargetFarthestOpponent(BaseUnit standardUnit)
    {
        var options = new UnitSearchOptions()
        {
            Group = TargetGroup.Enemy,
            Priority = TargetPriority.Farthest
        };

        List<BaseUnit> farthestEnemies = GetUnits(standardUnit, options);
        return farthestEnemies.FirstOrDefault(); // 가장 먼 적 반환
    }
}
