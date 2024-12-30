using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetQuset : IQuestCondition
{

    void UpdateProgress(int targetId, int count);

}
