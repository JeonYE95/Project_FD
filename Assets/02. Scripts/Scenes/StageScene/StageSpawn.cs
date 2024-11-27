using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StageSpawn : MonoBehaviour
{
  

    //나중에 SpawnPoint와 연결


    public int monsterSpawnCount { get; private set; }
    public int playerSpawnCount { get; private set; }


    private void SpawnEntity(int pointGroup, int point, int ID)
    {
       
   

        //몬스터,캐릭터에서 죽었을 때 이벤트에 Dead 추가 


    }


    // 상속 등으로 분리 해 각각에서 처리 - 캐릭터, 몬스터
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
