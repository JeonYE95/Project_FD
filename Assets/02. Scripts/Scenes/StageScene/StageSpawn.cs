using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StageSpawn : MonoBehaviour
{
    [SerializeField] private List<List<Vector2>> playerSpawnPoints;
    [SerializeField] private List<List<Vector2>> monsterSpawnPoints;


    // private Dictionary<int, > placedCharacters = new Dictionary<int, >();



    public int monsterSpawnCount { get; private set; }
    public int playerSpawnCount { get; private set; }


    private void MonsterSpawnEntity(int pointGroup, int point, int monsterID)
    {
       
        //몬스터 소환

        Vector3 spawnPoint = monsterSpawnPoints[pointGroup][point];


        monsterSpawnCount++;
    
    }



    // 인터페이스로 분리 해 각각에서 처리 
    /*
      private void Dead(int identifier)
    {
        monsterSpawnCount--;
   
        if ((monsterSpawnCount == 0))
        {
            StageManager.EndWave();
        }
    }
     */



}
