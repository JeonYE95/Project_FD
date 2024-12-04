using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{


    public void SpawnCharacter(Unit unit)
    {

        Instantiate(Resources.Load<GameObject>($"Prefabs/Unit/{unit.Grade}/{unit.Name}"));


    }

    public void SpawnEnemy(Unit unit)
    {

        Instantiate(Resources.Load<GameObject>($"Prefabs/Enemy/{unit.Name}"));

    }



}
