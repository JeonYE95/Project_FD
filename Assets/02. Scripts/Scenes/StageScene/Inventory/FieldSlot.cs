using UnityEngine;
using UnityEngine.EventSystems;


public class FieldSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler,
    IPointerEnterHandler, IDragHandler
{

    private Vector3 _previousPosition;


    [SerializeField]
    private GameObject _character = null;
    public GameObject Character => _character;


    // 필드에 캐릭터가 있는지 
    public void SetCharacter(GameObject character)
    {
        _character = character;

        if (character != null)
        {
            // 유닛을 Characters 오브젝트의 자식으로 설정
            character.transform.SetParent(FieldManager.Instance.CharactersParent);

            // UI 위치를 기준으로 적절한 월드 좌표 계산
            Vector3 screenPos = transform.position;
            screenPos.z = 10f; // 카메라로부터의 거리 설정
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

           

            character.transform.position = worldPos;
        }

    }

    // 몇번째 필드인지 정보 저장
    public int Index { get; private set; }

    private void Start()
    {
        //자신의 순서를 인덱스로 자동 할당
        Index = transform.GetSiblingIndex();
    }

    public void DropCharacter(UnitInfo unitInfo)
    {

        Vector3 uiPosition = transform.position;

        // UI 좌표를 월드 좌표로 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiPosition.x, uiPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0; // 2D 게임이라면 z축을 0으로 설정


        //인벤토리에 있는 Unit 정보 받아서 필드에 소환
        _character = UnitManager.Instance.CreatePlayerUnit(unitInfo._unitData.ID);


        if (_character != null)
        {
            // 부모-자식 관계 설정
            _character.transform.SetParent(FieldManager.Instance.CharactersParent);

            // 위치 설정
            _character.transform.position = worldPosition;

            // 초기 위치 저장
            _previousPosition = worldPosition;


            InventoryManager.Instance.TrackFieldUnit(Index, unitInfo);
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        // 해당 오브젝트 드래그 하기 전 위치 저장
        if (_character != null)
        {
            _previousPosition = _character.transform.position;

            Vector3 mousePosition = eventData.position;
            mousePosition.z = Camera.main.nearClipPlane;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0;

            _character.transform.position = worldPosition;

        }

    }

    // 드래그 중일 때 매 프레임 호출
    public void OnDrag(PointerEventData eventData)
    {

        if (_character != null)
        {
            _character.transform.position = Camera.main.ScreenToWorldPoint(eventData.position);
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
                UnitInfo unitInfo = previewInfo.GetUnitInfo();
                DropCharacter(unitInfo);
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

                // 캐릭터 위치 업데이트
                Vector3 mousePos = transform.position;
                mousePos.z = 10f;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                _character.transform.SetParent(FieldManager.Instance.CharactersParent);
                _character.transform.position = worldPos;

            }

            //현재 필드에 캐릭터가 있는 경우 - 교환
            else
            {
                //유닛 교환
                GameObject tempCharacter = _character;
                _character = fromSlot._character;
                fromSlot._character = tempCharacter;

                //각 유닛 위치 업데이트
                Vector3 thisPos = transform.position;
                thisPos.z = 10f;
                Vector3 thisWorldPos = Camera.main.ScreenToWorldPoint(thisPos);
                _character.transform.SetParent(FieldManager.Instance.CharactersParent);
                _character.transform.position = thisWorldPos;

                Vector3 otherPos = fromSlot.transform.position;
                otherPos.z = 10f;
                Vector3 otherWorldPos = Camera.main.ScreenToWorldPoint(otherPos);
                tempCharacter.transform.SetParent(FieldManager.Instance.CharactersParent);
                tempCharacter.transform.position = otherWorldPos;
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
