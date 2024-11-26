using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
        OpenProcedure();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        CloseProcedure();
    }

    protected virtual void OpenProcedure()
    {

    }

    protected virtual void CloseProcedure()
    {
        
    }
}
