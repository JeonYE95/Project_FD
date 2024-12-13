using UnityEngine;
using UnityEngine.EventSystems;
using GSDatas;

public class FieldSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler,
    IPointerEnterHandler, IDragHandler
{

    private Vector3 _previousPosition;
    private Canvas _canvas; // Canvas 참조 캐싱용 

    [SerializeField]
    private GameObject _character = null;
    public GameObject Character => _character;

    // FieldSlot에 직렬화 필드 추가
    [SerializeField] private int _slotIndex;
    [SerializeField] private int _groupIndex;

    // 몇번째 필드인지 정보 저장
    public int Index => _slotIndex;
    public int GroupIndex => _groupIndex;

    private void Awake()
    {
        // Awake에서 InventoryManager에 자신을 등록
        InventoryManager.Instance.RegisterFieldSlot(this);
    }

    public void SetIndex(int index)
    {
        _slotIndex = index;

        if (index >= 0 && index <= 3)
            _groupIndex = 1;
        else if (index >= 4 && index <= 7)
            _groupIndex = 2;
        else if (index >= 8 && index <= 11)
            _groupIndex = 3;
        else if (index >= 12 && index <= 15)
            _groupIndex = 4;
        else
            return;
    }



    // 필드에 캐릭터가 있는지 
    // public void SetCharacter(GameObject character)
    // {
    //     _character = character;

    //     if (character != null)
    //     {
    //         // 유닛을 Characters 오브젝트의 자식으로 설정
    //         character.transform.SetParent(FieldManager.Instance.CharactersParent);

    //         // UI 요소의 월드 중심점 구하기
    //         character.transform.position = Extensions.GetUIWorldPosition(GetComponent<RectTransform>());
    //     }

    // }

    public void SetCharacter(GameObject character)
    {
        _character = character;

        if (character != null)
        {
            // 유효한 그룹 인덱스인지 확인
            if (_groupIndex < 0 || _groupIndex > 5)
            {
                Debug.LogError("Invalid group index. Must be between 1 and 4.");
                return;
            }

            // 그룹 
            string groupName = $"Group{_groupIndex}";
            Transform group = FieldManager.Instance.CharactersParent.Find(groupName);

            if (group == null)
            {
                Debug.LogError($"Group '{groupName}' not found under 'Characters'.");
                return;
            }

            // 유닛을 그룹에 배치
            character.transform.SetParent(group);

            character.transform.SetSiblingIndex(_groupIndex);

            // UI 요소의 월드 중심점 구하기
            character.transform.position = Extensions.GetUIWorldPosition(GetComponent<RectTransform>());
        }
    }




    public void DropCharacter(UnitData unitInfo)
    {

        // 최대 소환 가능 수 도달하면 소환 불가
        if (!InventoryManager.Instance.CanSummonUnit())
            return;


        //인벤토리에 있는 Unit 정보 받아서 필드에 소환
        _character = UnitManager.Instance.CreatePlayerUnit(unitInfo.ID);


        if (_character != null)
        {
            string groupName = $"Group{_groupIndex}";
            Transform group = FieldManager.Instance.CharactersParent.Find(groupName);

            // 부모-자식 관계 설정
            _character.transform.SetParent(group);


            // UI 요소의 월드 중심점 구하고 위치 설정
            Vector3 worldPosition = Extensions.GetUIWorldPosition(GetComponent<RectTransform>());

            // 위치 설정
            _character.transform.localPosition = worldPosition;

            // 초기 위치 저장
            _previousPosition = worldPosition;

            // 인벤토리에서 차감
            InventoryManager.Instance.subtractCharacter(unitInfo.name, 1);
            Debug.Log($"Unit {unitInfo.name} count decreased in inventory.");
            InventoryManager.Instance.TrackFieldUnit(Index, unitInfo);
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // 해당 오브젝트 드래그 하기 전 위치 저장
        if (_character != null)
        {
            _previousPosition = _character.transform.position;

            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>();
            }

            // UI 요소의 월드 중심점 구하기
            _character.transform.position = Extensions.GetMouseWorldPosition(_canvas, eventData.position);
        }


    }

    // 드래그 중일 때 매 프레임 호출
    public void OnDrag(PointerEventData eventData)
    {

        if (_character != null)
        {

            if (_canvas == null)
            {
                _canvas = GetComponentInParent<Canvas>();
            }


            _character.transform.position = Extensions.GetMouseWorldPosition(_canvas, eventData.position);
        }

    }


    //드롭 했을 때
    public void OnDrop(PointerEventData eventData)
    {

        // 인벤토리에서 드래그 된 경우
        CharacterSlot characterSlot = eventData.pointerDrag.GetComponent<CharacterSlot>();
        if (characterSlot != null && _character == null)
        {

            UnitPrevInfo previewInfo = InventoryManager.Instance.PreviewObject.GetComponent<UnitPrevInfo>();
            if (previewInfo != null)
            {
                UnitData unitdata = previewInfo.GetUnitData();
                DropCharacter(unitdata);

                // PreviewObject 비활성화 
                InventoryManager.Instance.PreviewObject.SetActive(false);
            }

        }

        // 필드에서 드래그 된 경우
        FieldSlot fromSlot = eventData.pointerDrag.GetComponent<FieldSlot>();
        if (fromSlot != null && fromSlot._character != null)
        {

            // 현재 필드에 캐릭터가 없는 경우 - 단순 이동
            if (_character == null)
            {
                // 캐릭터 이동
                _character = fromSlot._character;
                fromSlot._character = null;

                //그룹으로 유닛 위치 이동
                SetCharacter(_character);


            }

            //현재 필드에 캐릭터가 있는 경우 - 교환
            else
            {
                //유닛 교환
                GameObject tempCharacter = _character;
                _character = fromSlot._character;
                fromSlot._character = tempCharacter;

                //각 유닛 위치 업데이트
                SetCharacter(_character);
                fromSlot.SetCharacter(tempCharacter);
            }

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (_character != null)
        {
            // 유효한 위치에 드롭되지 않았다면 원위치
            CharacterPosReSet();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryManager.Instance.SelectSlot(this);
    }


    public void CharacterInit()
    {

        CharacterPosReSet();

    }

    private void CharacterPosReSet()
    {
        _character.transform.position = _previousPosition;

    }

    public void RemoveCharacter()           // 캐릭터 제거 및 데이터 초기화
    {
        if (_character != null)
        {
            Destroy(_character);

            _character = null;

            InventoryManager.Instance.UntrackFieldUnit(Index);
        }
    }
}
