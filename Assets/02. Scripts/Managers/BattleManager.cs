using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseCharacter> allCharacters = new List<BaseCharacter>();
    public List<BaseCharacter> players = new List<BaseCharacter>();
    public List<BaseCharacter> enemies = new List<BaseCharacter>();

    public int playerCount;
    public int enemyCount;

    private TargetingSystem targetingSystem;

    //살아있는 캐릭터와 죽어있는 캐릭터 용 리스트 필요??
    //어느정도는 필요한게 적들 다 죽거나 플레이어 다 죽으면 웨이브(배틀) 이 끝나야 함
    private int totalCharacterCount => players.Count + enemies.Count;  // 전체 캐릭터 수 자동집계

    public List<BaseCharacter> GetPlayers()
    {
        return players;
    }

    public List<BaseCharacter> GetEnemies()
    {
        return enemies;
    }

    //일단 많이 부를 일 없는 함수에는 LINQ 사용
    public List<BaseCharacter> GetAlivePlayers()
    {
        return players.Where(x => x .isLive).ToList();
    }

    public List<BaseCharacter> GetAliveEnemies()
    {
        return enemies.Where(x => x .isLive).ToList();
    }

    protected override void Awake()
    {
        base.Awake();

        targetingSystem = new TargetingSystem(this);
    }

    public void CharacterDie(BaseCharacter character)
    {
        if (character.isPlayerCharacter)
        {
            playerCount--;
        }
        else
        {
            enemyCount--;
        }

        if (enemyCount == 0)
        {
            WaveManager.Instance.Victroy();
        }
        else if (playerCount == 0)
        {
            WaveManager.Instance.Lose();
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleSetingAndStart();
        }
    }

    public void BattleSetingAndStart()
    {
        foreach (BaseCharacter character in players)
        {
            allCharacters.Add(character);
            //character.OnDieEvent += Wavema

            //WaveManager.Instance.Victory
        }

        foreach (BaseCharacter character in enemies)
        {
            allCharacters.Add(character);
        }

        foreach (BaseCharacter character in allCharacters)
        {
            character.ActiveCharacter();
        }

        playerCount = players.Count;
        enemyCount = enemies.Count;
    }

    public void RegisterCharacter(BaseCharacter character)
    {
        if (character.isPlayerCharacter)
        {
            players.Add(character);
        }
        else
        {
            enemies.Add(character);
        }
    }

    
    public BaseCharacter GetTargetClosestOpponent(BaseCharacter standardCharacter)
    {
        return targetingSystem.GetTargetClosestOpponent(standardCharacter);
    }

    public List<BaseCharacter> GetCharacters(BaseCharacter standardCharacter, CharacterSearchOptions options)
    {
        return targetingSystem.GetCharacters(standardCharacter, options);
    }
}
