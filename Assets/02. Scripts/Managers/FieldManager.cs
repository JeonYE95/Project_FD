using UnityEngine;
using System.Collections.Generic;
public class FieldManager : Singleton<FieldManager>
{
    [SerializeField] private List<FieldSlot> _fieldSlots;

    [SerializeField] private Transform _charactersParent; //실제 유닛 담을 빈 게임오브젝트
    private Dictionary<int, Vector3> _fieldPositions = new Dictionary<int, Vector3>(); // 인게임에서 보일 필드 위치 저장
    private Dictionary<string, Transform> _characterGroups = new Dictionary<string, Transform>();


    public Transform CharactersParent => _charactersParent;


    protected override void Awake()
    {
        base.Awake();
        InitializeCharactersParent();
    }

    private void Start()
    {
        _fieldSlots = InventoryManager.Instance.FieldSlots;
        InitializeFieldPositions();
    }


    public bool HasUnitInField(int unitID)      // 유닛ID 필드에서 존재하는지 확인
    {
        foreach (var slot in _fieldSlots)
        {
            if (slot.Character != null)
            {
                var unitInfo = slot.Character.GetComponent<UnitInfo>();
                if (unitInfo != null && unitInfo._unitData.ID == unitID)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void RemoveUnitFromField(int unitID)     //특정 유닛ID 필드에서 제거
    {

        foreach (var slot in _fieldSlots)
        {
            if (slot.Character != null)
            {

                var unitInfo = slot.Character.GetComponent<UnitInfo>();
                if (unitInfo != null && unitInfo._unitData.ID == unitID)
                {

                    int slotIndex = slot.Index;

                    InventoryManager.Instance.UntrackFieldUnit(slotIndex);
                    slot.RemoveCharacter();

                    return;
                }
            }
        }
     
    }

    public void AddUnitToField(int unitID)      // 필드에 유닛 생성 배치
    {
        var unitData = UnitDataManager.Instance.GetUnitData(unitID);
        if (unitData == null) return;

        GameObject unitInstance = UnitManager.Instance.CreatePlayerUnit(unitID);
        if (unitInstance == null) return;

        if (_fieldSlots == null)
        {
            _fieldSlots = InventoryManager.Instance.FieldSlots;
        }

        foreach (var slot in _fieldSlots)
        {
            if (slot.Character == null)
            {
 
                slot.SetCharacter(unitInstance);
                InventoryManager.Instance.TrackFieldUnit(slot.Index, unitData);

                return;
            }
        }
    }

    public bool CanAddUnitToField()
    {

        if (_fieldSlots == null)
        {
            _fieldSlots = InventoryManager.Instance.FieldSlots;
        }

        foreach (var slot in _fieldSlots)
        {
            if (slot.Character == null) return true;
        }
        return false;
    }



    private void InitializeCharactersParent()
    {
        if (_charactersParent == null)
        {
            _charactersParent = new GameObject("Characters").transform;
        }

        // 하위 오브젝트 4개 생성
        string[] subGroups = { "Group1", "Group2", "Group3", "Group4" };

        int posZ = 14;
        foreach (string groupName in subGroups)
        {
            if (!_characterGroups.ContainsKey(groupName))
            {
                // 없으면 새로 생성
                GameObject newGroup = new GameObject(groupName);
                newGroup.transform.position = new Vector3(0, 0, posZ);
                newGroup.transform.SetParent(_charactersParent);

                _characterGroups[groupName] = newGroup.transform;
            }
            posZ -= 1;
        }
    }



    private void InitializeFieldPositions()
    {
        foreach (var slot in _fieldSlots)
        {
            Vector3 worldPos = GetFieldPosition(slot);
            _fieldPositions[slot.Index] = worldPos;
        }
    }


    private Vector3 GetFieldPosition(FieldSlot slot)
    {

        Vector3 screenPos = slot.transform.position;
        screenPos.z = 10f;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        return worldPos;
    }

    public Transform GetGroupTransform(string groupName)
    {
        if (_characterGroups.TryGetValue(groupName, out Transform group))
        {
            return group;
        }
        return null;
    }

}