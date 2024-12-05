using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{


    public UIInGameTest uiInGame;

    // Start is called before the first frame update
    void Start()
    {
        uiInGame = UIManager.Instance.GetUI<UIInGameTest>();
    }

    
}
