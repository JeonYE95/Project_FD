using UnityEngine;
using System.Collections.Generic;
using GSDatas;
using System;

public class PieceGacha : MonoBehaviour
{
    private int _gacha1Cost = 200;
    private int _gacha2Cost = 2000;
    private int _diamond;

    public void PlayPieceGacha()
    {
        _diamond = GameManager.Instance.playerData.diamond;

        if (_diamond < _gacha1Cost)
        {
            Debug.LogWarning( "다이아가 부족합니다" );
            return;
        }

        _diamond -= _gacha1Cost;
        GameManager.Instance.playerData.diamond = _diamond;

        Debug.Log($"다이아를 {_gacha1Cost}만큼 사용했습니다. 남은 다이아 : {_diamond}");

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
        _diamond = GameManager.Instance.playerData.diamond;
        
        if (_diamond < _gacha2Cost)
        {
            Debug.LogWarning("다이아가 부족합니다");
            return;
        }

        Debug.Log($"다이아를 {_gacha2Cost}만큼 사용했습니다. 남은 다이아 : {_diamond}");

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