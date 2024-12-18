using UnityEngine;

public class UIBase : MonoBehaviour
{
    public bool IsOpen { get; private set; }
    public void Open()
    {
        gameObject.SetActive(true);
        IsOpen = true;
        OpenProcedure();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        IsOpen = false;
        CloseProcedure();
    }

    protected virtual void OpenProcedure()
    {

    }

    protected virtual void CloseProcedure()
    {
        
    }
}
