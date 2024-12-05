using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseUnit> allUnits = new List<BaseUnit>();
    public List<BaseUnit> players = new List<BaseUnit>();
    public List<BaseUnit> enemies = new List<BaseUnit>();

    public int playerCount;
    public int enemyCount;

    private TargetingSystem targetingSystem;

    //살아있는 캐릭터와 죽어있는 캐릭터 용 리스트 필요??
    //어느정도는 필요한게 적들 다 죽거나 플레이어 다 죽으면 웨이브(배틀) 이 끝나야 함
    private int totalUnitCount => players.Count + enemies.Count;  // 전체 캐릭터 수 자동집계

    public List<BaseUnit> GetPlayers()
    {
        return players;
    }

    public List<BaseUnit> GetEnemies()
    {
        return enemies;
    }

    protected override void Awake()
    {
        base.Awake();

        targetingSystem = new TargetingSystem(this);
    }

    public void UnitDie(BaseUnit unit)
    {
        if (unit.isPlayerUnit)
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
        foreach (BaseUnit unit in players)
        {
            allUnits.Add(unit);
            //character.OnDieEvent += Wavema

            //WaveManager.Instance.Victory
        }

        foreach (BaseUnit unit in enemies)
        {
            allUnits.Add(unit);
        }

        foreach (BaseUnit unit in allUnits)
        {
            unit.ActiveUnit();
        }

        playerCount = players.Count;
        enemyCount = enemies.Count;
    }

    public void RegisterUnit(BaseUnit unit)
    {
        if (unit.isPlayerUnit)
        {
            players.Add(unit);
        }
        else
        {
            enemies.Add(unit);
        }
    }

    public void UnRegisterUnit(BaseUnit unit)
    {
        if (unit.isPlayerUnit)
        {
            players.Remove(unit);
        }
        else
        {
            enemies.Remove(unit);
        }
    }

    
    public BaseUnit GetTargetClosestOpponent(BaseUnit standardUnit)
    {
        return targetingSystem.GetTargetClosestOpponent(standardUnit);
    }

    public List<BaseUnit> GetUnits(BaseUnit standardUnit, UnitSearchOptions options)
    {
        return targetingSystem.GetUnits(standardUnit, options);
    }
}
