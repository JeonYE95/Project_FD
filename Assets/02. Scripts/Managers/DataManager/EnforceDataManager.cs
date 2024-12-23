using GSDatas;
using System.Collections.Generic;
using UnityEngine;

public class EnforceDataManager : MonoBehaviour
{
    private static EnforceDataManager _instance;
    public static EnforceDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EnforceDataManager();
            }
            return _instance;
        }
    }

    private EnforceDataManager() { }


}
