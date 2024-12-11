using Assets.HeroEditor.Common.Scripts.Common;
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

    public int alivePlayerUnitsCount;
    public int aliveEnemyUnitsCount;

    private TargetingSystem targetingSystem;
    
    private int totalUnitCount => players.Count + enemies.Count;  // 전체 캐릭터 수

    readonly int BattleResultAndResetTime = 3;
    WaitForSeconds _battleResultAndResetTimer;

    protected override void Awake()
    {
        base.Awake();

        _battleResultAndResetTimer = new WaitForSeconds((int)BattleResultAndResetTime);

        targetingSystem = new TargetingSystem(this);
    }

    private void Start()
    {
        //Debug.Log(SkillDataManager.Instance.GetSkillByUnitID(1001).SkillType);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleSetingAndStart();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestSpawn();
        }
    }
    public void UnitDie(BaseUnit unit)
    {
        if (unit is PlayerUnit)
        {
            alivePlayerUnitsCount--;
        }
        else if (unit is EnemyUnit)
        {
            aliveEnemyUnitsCount--;
        }

        CheckBattleResult();
    }

    private void CheckBattleResult()
    {
        //끝나면 그상태로 결과창 띄우고 결과창 확인하고나면 리셋이 좋긴한데
        //그러면 또 어려워짐
        if (aliveEnemyUnitsCount == 0)
        {
            Debug.Log("플레이어 승리");
            StartCoroutine(Victory());
        }
        else if (alivePlayerUnitsCount == 0)
        {
            Debug.Log("플레이어 패배");
            StartCoroutine(Lose());
        }
    }

    private IEnumerator Victory()
    {
        yield return _battleResultAndResetTimer;
        BattleEnd();
        //WaveManager.Instance.Victroy();
    }

    private IEnumerator Lose()
    {
        yield return _battleResultAndResetTimer;
        BattleEnd();
        //WaveManager.Instance.Lose();
    }

    private void BattleEnd()
    {
        foreach (var unit in allUnits)
        {
            unit.isLive = false;
            unit.SetActive(false);
        }

        foreach (var unit in enemies)
        {
            unit.UnregisterFromBattleManager();
        }

        //몬스터의 등록해제 타이밍이 문제
        //몬스터는 걍 죽을때마다 지가 해제해
        //ㄴㄴ 여기서 하는게 맞는데 구현만 분리해

        allUnits.RemoveAll(unit => enemies.Contains(unit));
        enemies.Clear();
    }

    public void ResetAllUnit()
    {
        foreach(BaseUnit unit in players)
        {
            unit.ReSetUnit();
        }
        
        Debug.Log("ResetAllUnit");
    }

    private void TestSpawn()
    {
        for (int i = 0; i < 1; i++)
        {
            var PlayerUnitList = UnitDataManager.GetList();

            //int randomNumber = PlayerUnitList[Random.Range(0, PlayerUnitList.Count)].ID;
            int randomNumber = Random.Range(1001, 1006);

            GameObject obj = UnitManager.Instance.CreatePlayerUnit(randomNumber);
            obj.transform.position = new Vector2(-5, 0);

            var EnemyUnitList = EnemyDataManager.GetList();

            randomNumber = EnemyUnitList[Random.Range(0, EnemyUnitList.Count)].ID;

            GameObject obj2 = EnemyManager.Instance.CreateEnemy(randomNumber);
            obj2.GetComponent<EnemyUnit>().RegisterToBattleManager();
            obj2.transform.position = new Vector2(5, 0);
        }
    }

    public void BattleSetingAndStart()
    {
        foreach (BaseUnit unit in players)
        {
            allUnits.Add(unit);
        }

        foreach (BaseUnit unit in enemies)
        {
            allUnits.Add(unit);
        }

        foreach (BaseUnit unit in allUnits)
        {
            unit.UnitBattleStart();
        }

        alivePlayerUnitsCount = players.Count;
        aliveEnemyUnitsCount = enemies.Count;
    }

    public void RegisterUnit(BaseUnit unit)
    {
        if(unit as PlayerUnit)
        {
            players.Add(unit);
        }
        else if (unit as EnemyUnit)
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
