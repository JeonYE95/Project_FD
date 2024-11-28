using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class CombinationData
{
    public string resultUnit;            // 조합 결과 유닛 ID
    public List<int> requiredUnits;     // 조합식 필요 유닛 모음
    public bool isHidden;               // 조합식 잠금 여부

    public static List<CombinationData> ParseList(string csvData)
    {
        List<CombinationData> combinations = new List<CombinationData>();
        string[] rows = csvData.Split('\n');

        for (int i = 1; i < rows.Length; i++) 
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            string[] columns = rows[i].Split(',');

            if (columns.Length < 6) continue;

            CombinationData combination = new CombinationData
            {
                resultUnit = columns[1].Trim(),
                requiredUnits = new List<int>
                {
                    int.Parse(columns[2]),
                    int.Parse(columns[3]),
                    int.TryParse(columns[4], out int req3) ? req3 : 0
                },
                isHidden = bool.Parse(columns[5].Trim())
            };
            combinations.Add(combination);
        }

        return combinations;
    }
}

