using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public int UnitID;
    public string Name;
    public int Attack;
    public int Defense;
    public int Health;

    public static List<UnitData> ParseList(string csvData)
    {
        List<UnitData> units = new List<UnitData>();
        string[] rows = csvData.Split('\n');

        for (int i = 1; i < rows.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(rows[i])) continue;

            string[] columns = rows[i].Split(',');

            if (columns.Length < 5)
            {
                Debug.LogWarning($"Row {i} has insufficient columns: {rows[i]}");
                continue;
            }

            try
            {
                UnitData unit = new UnitData
                {
                    UnitID = int.TryParse(columns[0], out int unitID) ? unitID : 0,
                    Name = columns[1].Trim(),
                    Attack = int.TryParse(columns[2], out int attack) ? attack : 0,
                    Defense = int.TryParse(columns[3], out int defense) ? defense : 0,
                    Health = int.TryParse(columns[4], out int health) ? health : 0
                };

                units.Add(unit);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing row {i}: {rows[i]} - {ex.Message}");
            }
        }

        return units;
    }
}
