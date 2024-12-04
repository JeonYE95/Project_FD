using System.Collections;
using System.Collections.Generic;
using System.IO;
using UGS;
using UnityEngine;

public class DataManager : SingletonDontDestory<DataManager>
{
    public UnitManager Units;

    private static DataManager _instance;


    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Units = new UnitManager();
    }
}

