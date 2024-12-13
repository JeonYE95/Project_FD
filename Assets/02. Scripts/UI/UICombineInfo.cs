using GSDatas;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICombineInfo : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Image _resultUnitImage1;
    [SerializeField] private Image _requiredUnit1Image1;
    [SerializeField] private Image _requiredUnit2Image1;

    [SerializeField] private Image _resultUnitImage2;
    [SerializeField] private Image _requiredUnit1Image2;
    [SerializeField] private Image _requiredUnit2Image2;

    private CombineData curCombineData;
        
    void Start()
    {
        _closeBtn.onClick.AddListener(() => { Close(); });
    }

    public void OnUnitClicked(int clickUnitId)
    {
        var combineDataList = CombineDataManager.Instance.GetRequiredUnitData(clickUnitId);

        if (combineDataList != null && combineDataList.Count > 0)
        {
            UpdateUI(combineDataList);
        }
    }

    private void UpdateUI(List<CombineData> combineDataList)
    {
        // 첫 번째 조합식 표시
        if (combineDataList.Count > 0)
        {
            UpdateCombineSlot(combineDataList[0], _requiredUnit1Image1, _requiredUnit2Image1, _resultUnitImage1);
        }

        // 두 번째 조합식 표시
        if (combineDataList.Count > 1)
        {
            UpdateCombineSlot(combineDataList[1], _requiredUnit1Image2, _requiredUnit2Image2, _resultUnitImage2);
        }
    }

    private void UpdateCombineSlot(CombineData combineData, Image requiredUnit1Image, Image requiredUnit2Image, Image resultUnitImage)
    {
        requiredUnit1Image.sprite = LoadUnitSprite(combineData.requiredunit1);
        requiredUnit2Image.sprite = LoadUnitSprite(combineData.requiredunit2);
        resultUnitImage.sprite = LoadUnitSprite(combineData.resultUnit);

        // 결과 유닛 이미지에 버튼 기능 추가
        var button = resultUnitImage.GetComponent<Button>();
        if (button != null)
        {
            // 버튼 클릭 시 조합 실행
            button.onClick.RemoveAllListeners(); // 이전 리스너 제거
            button.onClick.AddListener(() =>
            {
                CombineManager.Instance.ExecuteCombine(combineData.requiredunit1, combineData.requiredunit2);
                Close();
            });
        }
    }

    private Sprite LoadUnitSprite(int unitId)
    {
        var unitdata = UnitDataManager.Instance.GetUnitData(unitId);
        if (unitdata == null) return null;

        string spritePath = $"Sprite/Unit/UpperBody/{unitdata.grade}/{unitdata.name}";
        return Resources.Load<Sprite>(spritePath);
    }

    //private void OnResultUnitBtnCliked()
    //{
    //    if (curCombineData != null)
    //    {
    //        CombineManager.Instance.ExecuteCombine(curCombineData.requiredunit1, curCombineData.requiredunit2);
    //    }
    //}
}
