using GSDatas;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassUpgrade : MonoBehaviour
{

    public void UpgradeClass(string classType)
    {
        // 유닛 데이터 가져오기
        List<UnitData> classUnits = UnitDataManager.Instance.GetClassUnits(classType);

        if (classUnits == null || classUnits.Count == 0)
        {
            Debug.LogWarning($"클래스 타입 {classType}에 해당하는 유닛이 없습니다.");
            return;
        }

        // 현재 클래스 강화 레벨 확인
        int currentLevel = 0;
        if (GameManager.Instance.playerData.ClassEnforce.TryGetValue(classType, out int level))
        {
            currentLevel = level;
        }

        Debug.Log($"클래스 타입: {classType}, 현재 클래스 레벨: {currentLevel}");

        // 강화 레벨 조건 확인


        // 클래스 강화 에테르 확인
        ClassEnforceData classEnforceData = ClassEnforceDataManager.Instance.GetClassData(currentLevel);//클래스 레벨

        if (classEnforceData == null) return;   // 여기에서 끊김

        int curEther = GameManager.Instance.GetItemCount(3004);

        if (classEnforceData.requiredCost > curEther)
        {
            Debug.Log($"에테르 기운이 부족합니다. 요구조각 : {classEnforceData.requiredCost} , 현재 소유 에테르 : {curEther}");
            return;
        }


        // 유닛 데이터 적용
        foreach (var unit in classUnits)
        {
            // 유닛 강화 정보 불러오기
            int unitEnforceLevel = 0;
            GameManager.Instance.playerData.UnitEnforce.TryGetValue(unit.ID, out unitEnforceLevel);

            unit.attack += classEnforceData.attack;
            unit.defense += classEnforceData.defense;
            unit.health += classEnforceData.health;
            UnitDataManager.Instance.SaveClassData(unit);
        }

        // 클래스 강화 레벨 적용
        GameManager.Instance.playerData.ClassEnforce[classType] += 1;

        // 클래스 스탯 증가량 적용
        GameManager.Instance.playerData.ClassAddedData[classType].AddedAttackValue += classEnforceData.attack;
        GameManager.Instance.playerData.ClassAddedData[classType].AddedDefenseValue += classEnforceData.defense;
        GameManager.Instance.playerData.ClassAddedData[classType].AddedHealthValue += classEnforceData.health;

        //에테르 차감 및 저장
        GameManager.Instance.substractItemSave(3004, classEnforceData.requiredCost);

        // 강화 퀘스트 있다면 진행
        //QuestManager.Instance.UpdateEnforceQuests(0);


        Debug.Log($"클래스 타입:  {classType} , 현재 클래스 레벨:  {currentLevel + 1}");
    }
}
public class ClassAddedData
{
    public int AddedAttackValue = 0;
    public int AddedDefenseValue = 0;
    public int AddedHealthValue = 0;
}
