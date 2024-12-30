using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageCondition : IQuestCondition
{
    void UpdateProgress(int StageID, int ClearCount);
}
