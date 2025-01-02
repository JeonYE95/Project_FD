using UnityEngine;

public class UnitInfoInInspector : MonoBehaviour
{
    private IUnitInfo unitInfo;
    private InGameSkillData skillData;

    [Header("Unit Info (Inspector Only)")]
    [SerializeField] private int id;
    [SerializeField] private string unitName;
    [SerializeField] private float range;
    [SerializeField] private int attack;
    [SerializeField] private int health;
    [SerializeField] private int defense;
    [SerializeField] private float skillCooltime;
    [SerializeField] private float attackCooltime;
    [SerializeField] private string skillDescription;
    private void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
        skillData = GetComponent<SkillExecutor>().inGameSkillData;
        //GetComponent<SkillExecutor>().inGameSkillData.GetSkillDiscription();

        SyncUnitInfo(); // 초기 값 동기화
    }

    private void Update()
    {
        SyncUnitInfo(); // 매 프레임 값 동기화
    }

    private void SyncUnitInfo()
    {
        if (unitInfo == null) return;

        id = unitInfo.ID;
        unitName = unitInfo.Name;
        range = unitInfo.Range;
        attack = unitInfo.Attack;
        health = unitInfo.Health;
        defense = unitInfo.Defense;
        skillCooltime = unitInfo.SkillCooltime;
        attackCooltime = unitInfo.AttackCooltime;
        skillDescription = skillData.GetSkillDescription();
    }
}
