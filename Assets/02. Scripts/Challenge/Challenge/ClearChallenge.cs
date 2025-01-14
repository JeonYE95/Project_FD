using GSDatas;
using UnityEngine;

public class ClearChallenge : IChallengeStrategy
{
    public bool CheckCondition(challengeData challenge)
    {


        return StageManager.Instance.StageHealth >= 3;
       
    }

   
}
