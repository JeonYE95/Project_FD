using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class GradeIncludeChallenge :  IChallengeStrategy
{
    public bool CheckCondition(challengeData challenge)
    {


        List<BaseUnit> _endUnit = BattleManager.Instance._players;

        string requiredGrade = challenge.grade; 
        int requiredCount = challenge.requireCount;

        Defines.UnitGrade requiredUnitGrade;
        if (!System.Enum.TryParse(requiredGrade, true, out requiredUnitGrade))
        {
            return false;
        }

        // 해당 등급 이상 유닛 수 카운트
        int gradeCount = 0;
        foreach (var unit in _endUnit)
        {
            // UnitData 정보 가져오기
            UnitData unitData = UnitData.GetDictionary()[unit.ID];

            // 유닛의 등급을 enum으로 변환
            Defines.UnitGrade unitGrade;
            if (System.Enum.TryParse(unitData.grade, true, out unitGrade))
            {
                // enum 값 비교로 등급 이상 체크 (enum은 순서대로 0,1,2,3 값을 가짐)
                if (unitGrade >= requiredUnitGrade)
                {
                    gradeCount++;
                }
            }
        }

        // 요구 수량과 비교
        return gradeCount >= requiredCount;
    }
}

  
