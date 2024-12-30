using GSDatas;
using UnityEngine;

public class QuestDataManager : QuestData
{
    private static QuestDataManager _instance;
    public static QuestDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new QuestDataManager();
            }
            return _instance;
        }
    }


    public QuestData GetQuestData(int id)
    {
        if (QuestDataMap.TryGetValue(id, out var data))
        {
            return data;
        }

        return null;
    }
}
