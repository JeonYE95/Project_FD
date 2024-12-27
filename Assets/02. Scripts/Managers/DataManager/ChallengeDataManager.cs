using GSDatas;
using UnityEngine;

public class ChallengeDataManager : challengeData
{
    private static ChallengeDataManager _instance;
    public static ChallengeDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChallengeDataManager();
            }
            return _instance;
        }
    }
}
