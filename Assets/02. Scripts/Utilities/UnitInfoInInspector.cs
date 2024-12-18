using UnityEngine;

public class UnitInfoInInspector : MonoBehaviour
{
    private IUnitInfo unitInfo;

    [Header("Unit Info (Inspector Only)")]
    [SerializeField] private int id;
    [SerializeField] private string name;
    [SerializeField] private float range;
    [SerializeField] private int attack;
    [SerializeField] private int health;
    [SerializeField] private int defense;
    [SerializeField] private float skillCooltime;
    [SerializeField] private float attackCooltime;

    private void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
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
        name = unitInfo.Name;
        range = unitInfo.Range;
        attack = unitInfo.Attack;
        health = unitInfo.Health;
        defense = unitInfo.Defense;
        skillCooltime = unitInfo.SkillCooltime;
        attackCooltime = unitInfo.AttackCooltime;
    }
}
