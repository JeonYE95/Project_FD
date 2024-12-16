using UnityEngine;
using UnityEngine.UI;

public class CombineButton : MonoBehaviour
{
    [SerializeField] private int resultUnitID; // 조합 결과 유닛 ID

    public void OnClickCombine()
    {
        var combineData = CombineDataManager.Instance.GetCombineData(resultUnitID);

        if (combineData != null)
        {
            // 필요한 유닛 ID 가져오기
            int requiredUnit1Id = combineData.requiredunit1;
            int requiredUnit2Id = combineData.requiredunit2;

            // 조합 실행
            CombineManager.Instance.ExecuteCombine(requiredUnit1Id, requiredUnit2Id);
        }
        else
        {
            Debug.Log("조합 데이터를 찾을 수 없습니다!");
        }
    }

}
