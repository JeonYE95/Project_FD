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
            GameManager.Instance.playerData.ChallengeProgress.Add(challenge.Key, true);
            GameManager.Instance.progressSave();
        }
    }

    // 스테이지의 챌린지 완료 상태 가져오기
    public List<bool> GetStageChallengeStatus(int stageID)
    {
        List<bool> results = new List<bool>();
        if (_stageChallengeDic.TryGetValue(stageID, out var challenges))
        {
            foreach (challengeData challenge in challenges)
            {
                results.Add(GameManager.Instance.playerData.ChallengeProgress.ContainsKey(challenge.Key));
            }
        }
        return results;
    }


}