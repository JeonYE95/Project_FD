using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGachaQuestCondition : IQuestCondition
{
    void UpdateProgress(int count);
}
