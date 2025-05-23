using Assets.HeroEditor.Common.Scripts.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Random = UnityEngine.Random;

public class BattleManager : Singleton<BattleManager>
{
    bool _isBattleEnd = false;
    public bool isBattleRunning = false;
    readonly int BattleResultAndResetTime = 1;

    private TargetingSystem targetingSystem;
    WaitForSeconds _battleResultAndResetTimer;

    private List<BaseUnit> allUnits = new List<BaseUnit>();
    public List<BaseUnit> players = new List<BaseUnit>();
    public List<BaseUnit> enemies = new List<BaseUnit>();

    public List<BaseUnit> _players
    {
        get
        {
            return players;
        }
        set
        {
            _players = value;
        }
    }

    public ProjectileSO unitProjectileSO;
    public SkillVisualEffectPoolConfigSO skillVisualEffectSO;

    // 버프 딕셔너리 : <유닛, <버프이름, 버프 정보 >>
    private Dictionary<BaseUnit, Dictionary<string, BuffInfo>> activeBuffs = new Dictionary<BaseUnit, Dictionary<string, BuffInfo>>();

    public int alivePlayerUnitsCount;
    public int aliveEnemyUnitsCount;

    //For Debug
    public bool noDamageMode = false;
    Coroutine battleResultCoroutine;

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
        LoadBattleInfoConfig();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BattleSimulator();
        }
    }

    private static void BattleSimulator()
    {
        /*UIManager.Instance.Clear();
        UIManager.Instance.OpenUI<UIInGame>();*/

        InventoryManager.Instance.AddAllUnits();
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
        isBattleRunning = true;

        if (battleResultCoroutine != null)
        {
            StopCoroutine(battleResultCoroutine);
        }

        CheckBattleResult();
    }

    public void UnitDie(BaseUnit unit)
    {
        if (unit.isPlayerUnit)
        {
            alivePlayerUnitsCount--;
        }
        else
        {
            aliveEnemyUnitsCount--;

            // 적 처치 시 퀘스트 조건 확인
            QuestManager.Instance.UpdateKillQuests(unit.unitInfo.ID);


        }

        CheckBattleResult();
    }

    private void CheckBattleResult()
    {
        if (_isBattleEnd)
        {
            return;
        }

        //끝나면 그상태로 결과창 띄우고 결과창 확인하고나면 리셋이 좋긴한데
        //그러면 또 어려워짐
        if (aliveEnemyUnitsCount == 0 || CheckAllEnemyDie())
        {
            Debug.Log("플레이어 승리");

            battleResultCoroutine = StartCoroutine(Victory());
        }
        else if (alivePlayerUnitsCount == 0)
        {
            Debug.Log("플레이어 패배");
            battleResultCoroutine = StartCoroutine(Lose());
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
        isBattleRunning = false;

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
        isBattleRunning = false;

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

        //모든 버프 해제
        ResetAllBuff();

        foreach (var unit in enemies)
        {
            unit.UnregisterFromBattleManager();
        }

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
    public void SetAllUnits()
    {
        allUnits.Clear();


        foreach (BaseUnit unit in players)
        {
            //Debug.Log($"SetAllUnits - Players 리스트에 유닛 추가: {unit.gameObject.name} (ID: {unit.unitInfo?.ID})");
            allUnits.Add(unit);
        }

        foreach (BaseUnit unit in enemies)
        {
            //Debug.Log($"SetAllUnits - Enemies 리스트에 유닛 추가: {unit.gameObject.name} (ID: {unit.unitInfo?.ID})");
            allUnits.Add(unit);
        }

        foreach(BaseUnit unit in allUnits)
        {
            unit.ResetCoolTime();
        }
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

    private bool CheckAllEnemyDie()
    {
        foreach(BaseUnit unit in enemies)
        {
            if (unit.isLive)
            {
                return false;
            }
        }

        return true;
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

            //Debug.Log($"{target.gameObject.name} 기존 {buffKey} 버프 제거됨");
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

    private void LoadBattleInfoConfig()
    {
        // Resources 폴더에서 SkillVisualEffectSO 로드
        skillVisualEffectSO = Resources.Load<SkillVisualEffectPoolConfigSO>("Config/SkillVisualEffectSO");
        unitProjectileSO = Resources.Load<ProjectileSO>("Config/ProjectileSO");

        if (skillVisualEffectSO == null || unitProjectileSO == null)
        {
            Debug.LogError("스킬 SO 읽어오기 실패");
        }
        else
        {
            Debug.Log("스킬 SO 읽어오기 성공");
        }
    }

    // SkillEffectEntry를 가져오는 메서드
    public SkillVisualEffectEntry GetSkillEffect(int skillID)
    {
        // SkillVisualEffectPoolConfigSO가 null인지 확인
        if (skillVisualEffectSO == null)
        {
            Debug.LogError("SkillEffectPoolConfig is not loaded!");
            return null;
        }

        // skillEffects 리스트가 null이거나 비어 있는지 확인
        if (skillVisualEffectSO.skillEffects == null || skillVisualEffectSO.skillEffects.Count == 0)
        {
            Debug.LogError("SkillEffects list is null or empty!");
            return null;
        }

        // 스킬 ID로 효과를 찾기
        var effect = skillVisualEffectSO.skillEffects.Find(e => e.skillID == skillID);

        // 효과가 null인지 확인
        if (effect == null)
        {
            Debug.Log($"No skill visual effect found for skillID: {skillID}");
            effect = skillVisualEffectSO.NoneEffect;
        }

        return effect;
    }


    // ProjectileSprite 가져오는 메서드
    public ProjectileData GetProjectileSprite(int unitID)
    {
        if (unitProjectileSO.projectileDatas == null || unitProjectileSO.projectileDatas.Count == 0)
        {
            Debug.LogError("ProjectileSprite is not loaded!");
            return unitProjectileSO?.defaultProjectile;
        }

        var unitProjectile = unitProjectileSO.projectileDatas.Find(p => p.unitID == unitID);

        if (unitProjectile == null)
        {
            unitProjectile = unitProjectileSO.defaultProjectile;
        }

        return unitProjectile;
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
