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

    //살아있는 캐릭터와 죽어있는 캐릭터 용 리스트 필요??
    //어느정도는 필요한게 적들 다 죽거나 플레이어 다 죽으면 웨이브(배틀) 이 끝나야 함
    private int totalUnitCount => players.Count + enemies.Count;  // 전체 캐릭터 수 자동집계

    protected override void Awake()
    {
        base.Awake();

        targetingSystem = new TargetingSystem(this);
    }

    public static InGameSkillData _0111 = new InGameSkillData()
    {
        skillID = 111,
        unitID = 1001,
        skillName = "GuardianShield",
        skillType = SkillType.Buff,
        skillEffect = SkillEffect.DefenseBoost,
        value = 30.0f,
        duration = 5.0f,
        skillCoolDown = 10.0f,

        targetGroup = TargetGroup.Self,
        targetPriority = TargetPriority.All,
        targetCount = 1
    };

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
        yield return new WaitForSeconds(1f);
        BattleEnd();
        WaveManager.Instance.Victroy();
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(1f);
        BattleEnd();
        WaveManager.Instance.Lose();
    }

    private void BattleEnd()
    {
        var unitsToRemove = enemies.ToList(); // 순회 도중 오류 발생 방지
        foreach (BaseUnit unit in unitsToRemove)
        {
            unit.UnregisterFromBattleManager();
        }
    }


    private void ResetAllUnit()
    {
        //몬스터는 소환될때 리셋되고 배틀 끝나면 다른 몬스터 써서 리셋 필요없음
        foreach(BaseUnit unit in players)
        {
            unit.ReSetUnit();
        }
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
            //character.OnDieEvent += Wavema

            //WaveManager.Instance.Victory
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
