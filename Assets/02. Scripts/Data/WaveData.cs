using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveData
{
    public int StageLevel;
    public int WaveLevel;
    public int SpawnPoint;
    public int MonsterID;
  

    public static List<WaveData> ParseList(string csvData)
    {
        List<WaveData> waves = new List<WaveData>();
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
                WaveData wave = new WaveData
                {
                    StageLevel = int.TryParse(columns[0], out int StageLevel) ? StageLevel : 0,
                    WaveLevel = int.TryParse(columns[1], out int WaveLevel) ? WaveLevel : 0,
                    SpawnPoint = int.TryParse(columns[2], out int SpawnPoint) ? SpawnPoint : 0,
                    MonsterID = int.TryParse(columns[3], out int MonsterID) ? MonsterID : 0,
        
                };

                waves.Add(wave);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing row {i}: {rows[i]} - {ex.Message}");
            }
        }

        return waves;
    }
}
