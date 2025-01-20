using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IChallengeStrategy 
{
    bool CheckCondition(challengeData challenge);
}
