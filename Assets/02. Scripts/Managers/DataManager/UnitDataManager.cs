using GSDatas;
using System.Collections.Generic;
using System.Linq;

public class UnitDataManager : UnitData
{
    private static UnitDataManager _instance;
    public static UnitDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UnitDataManager();
            }
            return _instance;
        }
    }

    private UnitDataManager() {}

    public List<UnitData> GetUnitDatas()
    {
        return GetList();
    }

    public UnitData GetUnitData(int id)
    {
        if (UnitDataMap.TryGetValue(id, out var data))
        {
            return data;
        }
        
        return null;
    }

    public UnitData GetClassData(int id)
    {
        if (UnitDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }

    public List<UnitData> GetClassUnits(string classType)
    {
        return GetList().Where(unit => unit.classtype == classType).ToList();
    }

    public void SaveUnitData(UnitData unit)
    {
        if (GameManager.Instance.playerData.UnitEnforce == null)
        {
            GameManager.Instance.playerData.UnitEnforce = new Dictionary<int, int>();
        }

        if (GameManager.Instance.playerData.UnitEnforce.ContainsKey(unit.ID))
        {
            GameManager.Instance.playerData.UnitEnforce[unit.ID] = unit.level;
        }
        else
        {
            GameManager.Instance.playerData.UnitEnforce.Add(unit.ID, unit.level);
        }
    }


    public void SaveClassData(UnitData unit)
    {
        if (GameManager.Instance.playerData.ClassEnforce == null)
        {
            GameManager.Instance.playerData.ClassEnforce = new Dictionary<string, int>();
        }


        int classLevel = 0;
        if (GameManager.Instance.playerData.ClassEnforce.TryGetValue(unit.classtype, out int currentLevel))
        {
            classLevel = currentLevel;
        }

        // Dictionary에 클래스 레벨 저장/업데이트
        if (GameManager.Instance.playerData.ClassEnforce.ContainsKey(unit.classtype))
        {
            GameManager.Instance.playerData.ClassEnforce[unit.classtype] = classLevel;
        }
        else
        {
            GameManager.Instance.playerData.ClassEnforce.Add(unit.classtype, classLevel);
        }
        

    }
}