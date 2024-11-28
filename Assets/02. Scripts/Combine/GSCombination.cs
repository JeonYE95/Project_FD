using System.Collections.Generic;

public class GSCombination : ICombination
{
    private readonly List<string> requiredUnitID;    // 필요 유닛 ID
    private readonly string resultUnitID;             // 결과 유닛 ID
    private readonly bool isHidden;                 // 조합식 잠금 여부

    public GSCombination(List<string> requiredUnitID, string resultUnitID, bool isHidden)
    {
        this.requiredUnitID = requiredUnitID;
        this.resultUnitID = resultUnitID;
        this.isHidden = isHidden;
    }

    public bool CanCombine(List<string> inputUnitID)
    {
        return new HashSet<string>(inputUnitID).IsSupersetOf(requiredUnitID);     // 조합 재료 확인
    }

    public string Combine(List<string> inputUnitID)
    {
        if (CanCombine(inputUnitID))
        {
            return resultUnitID;          // 조합 성공
        }

        return null;                    // 조합 실패
    }
}