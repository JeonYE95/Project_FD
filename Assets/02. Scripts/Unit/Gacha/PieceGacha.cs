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

            //뽑기 퀘스트 진행
            QuestManager.Instance.UpdateGachaQuest(0);
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

    //private int GetPieceAmountByGrade(string grade)
    //{
    //    return _pieceAmount.ContainsKey(grade) ? _pieceAmount[grade] : 1;
    //}
}