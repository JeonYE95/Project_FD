using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQustClearCondition : IQuestCondition
{

    void UpdateProgress(int QuestID, int ClearCount);
}
