using UnityEngine.EventSystems;

public class UITutorial : UIBase
{

    private EventTrigger _eventTrigger;
    private EventTrigger.Entry _clickEntry;


    private void Start()
    {
        _eventTrigger = GetComponent<EventTrigger>();
        _clickEntry = new EventTrigger.Entry();
        _clickEntry.eventID = EventTriggerType.PointerClick;
        _clickEntry.callback.AddListener((data) => { Close(); });
        _eventTrigger.triggers.Add(_clickEntry);

    }


    private void OnEnable()
    {
        StageManager.Instance.StopGame();
    }

    public void OnDisable()
    {
        StageManager.Instance.ResumeGame();

    }

}
