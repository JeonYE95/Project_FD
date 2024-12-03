using System.Collections;
using System.Collections.Generic;
using System.IO;
using UGS;
using UnityEngine;

public class DataManager : SingletonDontDestory<GameManager>
{
    private static DataManager _instance;

    public static DataManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new DataManager();

            return _instance;
        }
    }

    public UnitManager Units;

    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Units = new UnitManager();
    }
}

