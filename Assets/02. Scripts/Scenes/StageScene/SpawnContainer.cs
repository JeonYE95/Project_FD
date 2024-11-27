using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnContainer : MonoBehaviour
{
     
    public List<SpawnPointGroup> SpawnPoint;
  

}

[Serializable]
public class SpawnPointGroup
{
    public string Name;
    public List<Transform> Points;
}