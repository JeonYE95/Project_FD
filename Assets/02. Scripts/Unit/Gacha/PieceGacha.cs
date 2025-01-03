using UnityEngine;
using System.Collections.Generic;
using GSDatas;
using System;

public class PieceGacha : MonoBehaviour
{
    private int _gachaCost = 200;
    private int _diamond = 10000;

    public void PlayPieceGacha()
    {
        if (_diamond < _gachaCost)
        {
            Debug.LogWarning( "다이아가 부족합니다" );
            return;
        }

        _diamond -= _gachaCost;
        Debug.Log($"다이아를 {_gachaCost}만큼 사용했습니다. 남은 다이아 : {_diamond}");

        GachaData selectedUnit = GachaDataManager.Instance.GetRandomData("Outgame");

        if (selectedUnit != null)
        {
            int pieceAmount = selectedUnit.pieceamount;
            Debug.Log($"뽑힌 유닛: {selectedUnit.name}, 등급: {selectedUnit.grade}, 조각 수: {pieceAmount}");

            GameManager.Instance.AddItemSave(selectedUnit.ID, pieceAmount);

            TrackGachaResult(selectedUnit);
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

    private Dictionary<string, int> gradeCount = new Dictionary<string, int>();
    private int totalGachaCount = 0;

    public void TrackGachaResult(GachaData selectedUnit)
    {
        if (selectedUnit != null)
        {
            totalGachaCount++;

            // 등급별 카운트 증가
            if (gradeCount.ContainsKey(selectedUnit.grade))
            {
                gradeCount[selectedUnit.grade]++;
            }
            else
            {
                gradeCount[selectedUnit.grade] = 1;
            }
        }
    }

    public void PrintGachaStats()
    {
        Debug.Log("[ 누적 가챠 결과 ]");
        foreach (var grade in gradeCount)
        {
            float gradeProbability = (float)grade.Value / totalGachaCount * 100f;
            Debug.Log($"등급: {grade.Key}, 뽑힌 횟수: {grade.Value}, 확률: {gradeProbability:F2}%");
        }
    }
}