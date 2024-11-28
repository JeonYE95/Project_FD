using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class CombinationData
{
    public int combinationID;           // 조합식 ID
    public int resultUnitID;            // 조합 결과 유닛 ID
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

            if (columns.Length < 6)
            {
                Debug.LogError($"Row {i} has insufficient columns: {rows[i]}");
                continue;
            }

            try
            {
                CombinationData combination = new CombinationData
                {
                    combinationID = int.TryParse(columns[0], out int combinationID) ? combinationID : 0,
                    resultUnitID = int.TryParse(columns[1], out int resultUnitID) ? resultUnitID : 0,
                    requiredUnits = new List<int>
                {
                    int.TryParse(columns[2], out int req1) ? req1 : 0,
                    int.TryParse(columns[3], out int req2) ? req2 : 0,
                    int.TryParse(columns[4], out int req3) ? req3 : 0
                },
                    isHidden = bool.TryParse(columns[5].Trim(), out bool isHidden) ? isHidden : false
                };

                combinations.Add(combination);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing row {i}: {rows[i]} - {ex.Message}");
            }
        }

        return combinations;
    }
}

