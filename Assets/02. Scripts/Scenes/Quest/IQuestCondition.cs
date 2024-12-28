
public interface IQuestCondition
{
    bool CheckCondition();
    void UpdateProgress();
    void Reset();

    int GetCurrentProgress();
}
