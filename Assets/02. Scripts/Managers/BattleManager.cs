using Assets.HeroEditor.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    bool _isBattleEnd = false;
    readonly int BattleResultAndResetTime = 1;

    private TargetingSystem targetingSystem;
    WaitForSeconds _battleResultAndResetTimer;

    private List<BaseUnit> allUnits = new List<BaseUnit>();
    public List<BaseUnit> players = new List<BaseUnit>();
    public List<BaseUnit> enemies = new List<BaseUnit>();

    // 버프 딕셔너리 : <유닛, <버프이름, 버프 정보 >>
    private Dictionary<BaseUnit, Dictionary<string, BuffInfo>> activeBuffs = new Dictionary<BaseUnit, Dictionary<string, BuffInfo>>();

    public int alivePlayerUnitsCount;
    public int aliveEnemyUnitsCount;

    //For Debug
    public bool noDamageMode = false;

    public bool IsBattleEnd
    {
        get
        {
            return _isBattleEnd;
        }
        set
        {
            _isBattleEnd = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        targetingSystem = new TargetingSystem(this);
        _battleResultAndResetTimer = new WaitForSeconds((int)BattleResultAndResetTime);
    }

    private void Start()
    {
        //Debug.Log(SkillDataManager.Instance.GetSkillByUnitID(1001).SkillType);
    }

    public void BattleSettingAndStart()
    {
        SetAllUnits();

        foreach (BaseUnit unit in allUnits)
        {
            unit.UnitBattleStart();
        }

        alivePlayerUnitsCount = players.Count;
        aliveEnemyUnitsCount = enemies.Count;

        _isBattleEnd = false;
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

        //aliveEnemyUnitsCount = 0;
        //alivePlayerUnitsCount = 0;
    }

    public IEnumerator Victory()
    {
        if (_isBattleEnd)
        {
            yield break;
        }

        _isBattleEnd = true;

        yield return _battleResultAndResetTimer;
        BattleEnd();
        WaveManager.Instance.Victroy();
    }

    public IEnumerator Lose()
    {
        if (_isBattleEnd)
        {
            yield break;
        }

        _isBattleEnd = true;

        yield return _battleResultAndResetTimer;
        BattleEnd();
        WaveManager.Instance.Lose();
    }

    private void BattleEnd()
    {
        foreach (var unit in allUnits)
        {
            unit.isLive = false;
            unit.gameObject.SetActive(false);
        }

        foreach (var unit in enemies)
        {
            unit.UnregisterFromBattleManager();
        }

        //몬스터의 등록해제 타이밍이 문제
        //몬스터는 걍 죽을때마다 지가 해제해
        //ㄴㄴ 여기서 하는게 맞는데 구현만 분리해

        //모든 버프 해제
        ResetAllBuff();

        allUnits.RemoveAll(unit => enemies.Contains(unit));
        enemies.Clear();
    }

    public void ResetAllUnit()
    {
        foreach(BaseUnit unit in players)
        {
            unit.ReSetUnit();
            //unit.animController.ResetAnim();
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

    public void SetAllUnits()
    {
        allUnits.Clear();

        foreach (BaseUnit unit in players)
        {
            allUnits.Add(unit);
        }

        foreach (BaseUnit unit in enemies)
        {
            allUnits.Add(unit);
        }
    }

    

    public void RegisterUnit(BaseUnit unit)
    {
        if (unit is PlayerUnit)
        {
            players.Add(unit);
        }
        else if (unit is EnemyUnit)
        {
            enemies.Add(unit);
        }

        SetAllUnits();

    }

    public void UnRegisterUnit(BaseUnit unit)
    {
        if (unit.isPlayerUnit && players.Contains(unit))
        {
            players.Remove(unit);
        }
        else if (enemies.Contains(unit))
        {
            enemies.Remove(unit);
        }

        SetAllUnits();
    }


    public void ApplyBuff(BaseUnit target, string buffKey, float duration, Action applyAction, Action resetAction)
    {
        if (target == null)
        {
            Debug.Log($"{buffKey} 의 대상 타겟이 null 입니다");
            return;
        }

        // 타겟 유닛이 딕셔너리에 없으면 새로 생성
        if (!activeBuffs.ContainsKey(target))
        {
            activeBuffs[target] = new Dictionary<string, BuffInfo>();
        }

        // 기존 버프가 있다면 기존 리셋 액션 호출 및 코루틴 중단
        if (activeBuffs[target].ContainsKey(buffKey))
        {
            var existingBuff = activeBuffs[target][buffKey]; //리셋 액션 추출

            existingBuff.ResetAction?.Invoke(); // 리셋 액션 실행
            StopCoroutine(existingBuff.Timer);  // 기존 코루틴 중단
            activeBuffs[target].Remove(buffKey); // 딕셔너리에서 제거

            Debug.Log($"{target.gameObject.name} 기존 {buffKey} 버프 제거됨");
        }

        // 새 버프 적용
        applyAction?.Invoke();
        Debug.Log($"{target.gameObject.name}에게 {buffKey} 버프 적용됨.");

        // 새로운 타이머 및 리셋 액션 등록
        Coroutine timer = StartCoroutine(BuffTimer(target, buffKey, duration));
        activeBuffs[target][buffKey] = new BuffInfo(timer, resetAction);
    }


    //버프 코루틴 함수
    private IEnumerator BuffTimer(BaseUnit target, string buffKey, float duration)
    {
        yield return new WaitForSeconds(duration);

        if (activeBuffs.ContainsKey(target) && activeBuffs[target].ContainsKey(buffKey))
        {
            var buffInfo = activeBuffs[target][buffKey];

            buffInfo.ResetAction?.Invoke(); // 버프 리셋 실행
            activeBuffs[target].Remove(buffKey); // 버프 제거

            // 유닛의 모든 버프가 제거되면 유닛 자체 딕셔너리에서 제거
            if (activeBuffs[target].Count == 0)
            {
                activeBuffs.Remove(target);
                Debug.Log($"{target.gameObject.name}의 모든 버프가 정리되었습니다.");
            }
        }
        else
        {
            Debug.Log($"{target?.gameObject.name ?? "삭제된 유닛"}의 {buffKey} 버프 리셋 처리 완료 (null 상태 포함).");
        }
    }

    private void ResetAllBuff()
    {
        foreach (var unit in activeBuffs.Keys)
        {
            foreach (var buff in activeBuffs[unit].Values)
            {
                if (buff.Timer != null)
                {
                    StopCoroutine(buff.Timer);
                }

                buff.ResetAction?.Invoke();
            }
        }

        activeBuffs.Clear();
        Debug.Log("모든 버프 초기화 됨");
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
