using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class ClassIncludeChallenge : IChallengeStrategy
{
    public bool CheckCondition(challengeData challenge)
    {

        List<BaseUnit> _endUnits = BattleManager.Instance.players;

        string requiredClass = challenge.ClassType;
        int requiredCount = challenge.requireCount;

        // 해당 클래스 유닛 수 카운트
        int classCount = 0;

        foreach (var unit in _endUnits)
        {
            // UnitData 정보 가져오기
            UnitData unitData = UnitData.GetDictionary()[unit.ID];

            // 클래스 타입이 일치하는지 확인
            if (unitData.classtype == requiredClass)
            {
                classCount++;
            }
        }

        // 요구 수량과 비교
        return classCount >= requiredCount;
    }


}
