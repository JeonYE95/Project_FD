using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumeCondition : IQuestCondition
{
    bool CheckCondition();
    void UpdateProgress(int itemId, int count);

}
