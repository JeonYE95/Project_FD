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
        CloseProcedure();
        gameObject.SetActive(false);
        IsOpen = false;
    }

    protected virtual void OpenProcedure()
    {

    }

    protected virtual void CloseProcedure()
    {
        
    }
}
