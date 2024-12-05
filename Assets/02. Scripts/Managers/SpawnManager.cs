using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{


    public void SpawnCharacter(UnitInfo unit)
    {

        Instantiate(Resources.Load<GameObject>($"Prefabs/Unit/{unit._unitData.grade}/{unit._unitData.name}"));


    }

    public void SpawnEnemy(UnitInfo unit)
    {

        Instantiate(Resources.Load<GameObject>($"Prefabs/Enemy/{unit._unitData.name}"));

    }



}
