using GSDatas;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeManager : Singleton<ChallengeManager>
{
    private Dictionary<int, List<challengeData>> _stageChallengeDic = new Dictionary<int, List<challengeData>>();
    public void Initialize()
    {
        // 스테이지별 도전과제 데이터 정리
        List<challengeData> allChallenges = challengeData.GetList();
        foreach (challengeData challenge in allChallenges)
        {
            if (!_stageChallengeDic.ContainsKey(challenge.ID))
            {
                _stageChallengeDic[challenge.ID] = new List<challengeData>();
            }
            _stageChallengeDic[challenge.ID].Add(challenge);
        }
    }

    // 스테이지 클리어 시 호출
    public void CheckStageChallenges(int stageID)
    {
        if (_stageChallengeDic.TryGetValue(stageID, out var challenges))
        {
            foreach (var challenge in challenges)
            {
                // 챌린지 조건 체크 및 완료 처리
                if (CheckChallengeCondition(challenge))
                {
                    CompleteChallenege(challenge);
                }
            }
        }



    }

    private bool CheckChallengeCondition(challengeData challenge)
    {
        // 여기에 각 챌린지 타입별 조건 체크 로직 구현


        return false;
    }

    private void CompleteChallenege(challengeData challenge)
    {
        if (!GameManager.Instance.playerData.ChallengeProgress.ContainsKey(challenge.Key))
        {

            //클리어 했다면 몇번째인지도 넣어야 하네. 


        }
        else
        {




        }

        GameManager.Instance.progressSave();
    }


}



public class ChallengeClearData
{

    public int Challenge_1 = (int)Defines.StageChallengeClearState.None;
    public int Challenge_2 = (int)Defines.StageChallengeClearState.None;
    public int Challenge_3 = (int)Defines.StageChallengeClearState.None;


}