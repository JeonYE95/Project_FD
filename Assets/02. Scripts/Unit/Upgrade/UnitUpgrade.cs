using GSDatas;
using UnityEngine;

public class UnitUpgrade : MonoBehaviour
{
    public void UpgradeUnit(int unitId)
    {
        // 유닛 데이터 가져오기
        UnitData unit = UnitDataManager.Instance.GetUnitData(unitId);
        Debug.Log($"유닛 레벨 : {unit.level}, 최대 레벨 : {unit.maxLevel}");
        Debug.Log($"유닛 : {unit.name}, 현재 레벨 : {unit.level}, 공격력 : {unit.attack}, 방어력 : {unit.defense}, 체력 : {unit.health}");

        // 강화 레벨 조건 확인
        if (unit.level >= unit.maxLevel)
        {
            Debug.Log($"{unit.name}은 최대 레벨에 도달했습니다.");
            return;
        }

        // 유닛 강화 조각 확인
        int requiredPieces = UnitEnforceDataManager.Instance.GetRequriedPieces(unit.grade, unit.level);
        int curPieces = GameManager.Instance.GetItemCount(unitId);

        if (requiredPieces > curPieces)
        {
            Debug.Log($"유닛 조각이 부족합니다. 요구조각 : {requiredPieces} , 현재조각 : {curPieces}");
            return;
        }

        // 강화 데이터 확인
        UnitEnforceData unitEnforceData = UnitEnforceDataManager.Instance.GetEnforceData(unit.grade, unit.level);

        if (unitEnforceData == null) return;

        // 데이터 적용
        unit.attack += unitEnforceData.attack;
        unit.defense += unitEnforceData.defense;
        unit.health += unitEnforceData.health;
        unit.level++;

        // 조각 차감 및 저장
        UnitDataManager.Instance.SaveUnitData(unit);
        GameManager.Instance.substractItemSave(unitId, requiredPieces);

        // 강화 퀘스트 진행
        QuestManager.Instance.UpdateEnforceQuests(0);


        Debug.Log($"업그레이드 완료, 유닛 : {unit.name}, 현재 레벨 : {unit.level}, 공격력 : {unit.attack}, 방어력 : {unit.defense}, 체력 : {unit.health}");
    }
}