
public interface IKillQuestCondition : IQuestCondition
{
    void UpdateProgress(int targetID, int killCount);
}
