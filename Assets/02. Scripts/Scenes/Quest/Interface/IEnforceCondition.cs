using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnforceCondition : IQuestCondition
{

    void UpdateProgress(int count);


}
