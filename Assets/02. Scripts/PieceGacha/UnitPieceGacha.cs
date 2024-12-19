using UnityEngine;
using System.Collections.Generic;
using GSDatas;
using System;

public class UnitPieceGacha : MonoBehaviour
{
    private int _gachaCost = 200;
    private int _diamond = 10000;

    private readonly Dictionary<string, int> _pieceAmount = new Dictionary<string, int>()
    {
        { "Common", 10 },
        { "Rare", 5 },
        { "Unique", 3 }
    };

    public void PlayPieceGacha()
    {
        if (_diamond < _gachaCost)
        {
            Debug.LogWarning( "다이아가 부족합니다" );
            return;
        }

        _diamond -= _gachaCost;
        Debug.Log($"다이아를 {_gachaCost}만큼 사용했습니다. 남은 다이아 : {_diamond}");

        UnitData selectedUnit = GetUnitByWeight();

        if (selectedUnit != null)
        {
            int pieceAmount = GetPieceAmountByGrade(selectedUnit.grade);
            Debug.Log($"뽑힌 유닛 : {selectedUnit.name} , 등급 : {selectedUnit.grade} , 조각 개수 : {pieceAmount}");
        }

    }

    public void PlayPieceGacha10()
    {
        int totalGachaCost = _gachaCost * 10;

        if (_diamond < totalGachaCost)
        {
            Debug.LogWarning("다이아가 부족합니다");
            return;
        }

        Debug.Log($"다이아를 {totalGachaCost}만큼 사용했습니다. 남은 다이아 : {_diamond}");

        for (int i = 0; i < 10; i++)
        {
            PlayPieceGacha();
        }
    }

    private UnitData GetUnitByWeight()
    {
        List<UnitData> unitList = UnitDataManager.Instance.GetUnitDatas();
        int totalWeight = 0;

        foreach (var unit in unitList)
        {
            totalWeight += unit.weight;
        }
        
        System.Random random = new System.Random();
        int randomValue = random.Next(1, totalWeight + 1);
        int cumulativeValue = 0;

        foreach (var unit in unitList)
        {
            cumulativeValue += unit.weight;
            if (randomValue <= cumulativeValue)
            {
                return unit;
            }
        }

        return null;
    }

    private int GetPieceAmountByGrade(string grade)
    {
        return _pieceAmount.ContainsKey(grade) ? _pieceAmount[grade] : 1;
    }
}