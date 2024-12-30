using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConsumeCondition : IQuestCondition
{
    void UpdateProgress(int itemId, int count);

}
