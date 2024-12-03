using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class CombinationData
{
    public string resultUnit;            // 조합 결과 유닛 ID
    public List<int> requiredUnits;     // 조합식 필요 유닛 모음
}

