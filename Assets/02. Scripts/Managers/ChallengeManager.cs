using GSDatas;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChallengeManager : Singleton<ChallengeManager>
{

    private Dictionary<string, IChallengeStrategy> challengeStrategies;

    private List<challengeData> _currentChallengeData;



    public void Initialize()
    {
        challengeStrategies = new Dictionary<string, IChallengeStrategy>
        {
            { "Clear", new ClearChallenge() },
            { "ClassInclude", new ClassIncludeChallenge() },
            { "GradeInclude", new GradeIncludeChallenge()},
        };

    }

    // 스테이지 클리어 시 호출
    public void UpdateStageChallenge(int stageID)
    {
        _currentChallengeData =  GetCurrentchallengeData(stageID);

        foreach (var challenge in _currentChallengeData)
        {
            if (challengeStrategies.TryGetValue(challenge.ChallengeType, out var strategy))
            {
                if (strategy.CheckCondition(challenge))
                {
                    CompleteChallenege(challenge);
                }
            }
            else
            {
                Debug.LogWarning($"Unknown challenge type: {challenge.ChallengeType}");
            }
        }


    }

    // 도전과제 상태 업데이트
    public void UpdateChallengeState(int stageId, int challengeNumber, Defines.StageChallengeClearState state)
    {
        if (!GameManager.Instance.playerData.ChallengeClearData.ContainsKey(stageId))
        {
            GameManager.Instance.playerData.ChallengeClearData[stageId] = new ChallengeClearData();
        }

        var challengeData = GameManager.Instance.playerData.ChallengeClearData[stageId];
        switch (challengeNumber)
        {
            case 1: challengeData.Challenge_1 = (int)state; break;
            case 2: challengeData.Challenge_2 = (int)state; break;
            case 3: challengeData.Challenge_3 = (int)state; break;
        }

        GameManager.Instance.progressSave(); 
    }

    // 도전과제 상태 확인
    public Defines.StageChallengeClearState GetChallengeState(int stageId, int challengeNumber)
    {
        var playerData = GameManager.Instance.playerData;
        if (playerData.ChallengeClearData.TryGetValue(stageId, out var challengeData))
        {
            return challengeNumber switch
            {
                1 => (Defines.StageChallengeClearState)challengeData.Challenge_1,
                2 => (Defines.StageChallengeClearState)challengeData.Challenge_2,
                3 => (Defines.StageChallengeClearState)challengeData.Challenge_3,
                _ => Defines.StageChallengeClearState.None
            };
        }
        return Defines.StageChallengeClearState.None;
    }


    private void CompleteChallenege(challengeData challenge)
    {


        int stageId = challenge.ID;  // 스테이지 ID
        int challengeNumber = challenge.Key % 3;

        // 해당 스테이지의 ChallengeClearData를 가져오거나 새로 생성
        if (!GameManager.Instance.playerData.ChallengeClearData.ContainsKey(stageId))
        {
            GameManager.Instance.playerData.ChallengeClearData[stageId] = new ChallengeClearData();
        }

        var challengeData = GameManager.Instance.playerData.ChallengeClearData[stageId];

        // 현재 도전과제의 상태 확인
        int currentState = challengeNumber switch
        {
            1 => challengeData.Challenge_1,
            2 => challengeData.Challenge_2,
            3 => challengeData.Challenge_3,
        };

        if (currentState != (int)Defines.StageChallengeClearState.Clear)
        {
            // 도전과제 클리어 처리
            switch (challengeNumber)
            {
                case 1: challengeData.Challenge_1 = (int)Defines.StageChallengeClearState.Clear; break;
                case 2: challengeData.Challenge_2 = (int)Defines.StageChallengeClearState.Clear; break;
                case 0: challengeData.Challenge_3 = (int)Defines.StageChallengeClearState.Clear; break;
            }
            Debug.Log($"스테이지 {stageId}의 도전과제 {challenge.Key}가 완료되었습니다.");

        }
        else
        {
            Debug.Log($"스테이지 {stageId}의 도전과제 {challenge.Key}는 이미 완료되었습니다.");
        }

    }


    private List<challengeData> GetCurrentchallengeData(int _stageID)
    {
        return challengeData.GetList().Where(data => data.ID == _stageID).ToList();
    }

}



public class ChallengeClearData
{

    public int Challenge_1 = (int)Defines.StageChallengeClearState.None;
    public int Challenge_2 = (int)Defines.StageChallengeClearState.None;
    public int Challenge_3 = (int)Defines.StageChallengeClearState.None;

}