using System.Collections.Generic;
using UnityEngine;


public class WaveSpawn : MonoBehaviour
{

    //나중에 SpawnPoint와 연결

    public int monsterRemainCount { get; private set; }
    public int playerRemainCount { get; private set; }



    public void Start()
    {
        
    }



    private void SpawnEntity(int point, int ID)
    {

        //몬스터,캐릭터에서 죽었을 때 이벤트에 Dead 추가 
        // DB에서 ID로 소환
        BaseCharacter character = new BaseCharacter();


        if (character.isPlayerCharacter)
        {
            character.OnDieEvent += () => monsterRemainCount--;

        }
        else 
        {
            character.OnDieEvent += () => playerRemainCount--;

        }
    }



}
