using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class FieldSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler,
    IPointerEnterHandler, IDragHandler
{

    private Vector3 _previousPosition;
    private Inventory _fieldSlot;

    [SerializeField]
    private GameObject _character = null;
    public GameObject Character => _character;


    // 필드에 캐릭터가 있는지 
    public void SetCharacter(GameObject character)
    {
        _character = character;
    }

    // 몇번째 필드인지 정보 저장
    public int Index { get; private set; }

    public void Init(Inventory FieldSlot, int index)
    {
        _fieldSlot = FieldSlot;
        Index = index;
    }

    //소환 부분 : SpawnManager를 만들고 불러오는걸로 수정 예정
    public void SpawnCharacter()
    {

        Vector3 uiPosition = transform.position;

        // UI 좌표를 월드 좌표로 변환
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiPosition.x, uiPosition.y, Camera.main.nearClipPlane));
        worldPosition.z = 0; // 2D 게임이라면 z축을 0으로 설정

        _character = Instantiate(Resources.Load<GameObject>("Prefabs/Unit/Common/Archer"), worldPosition, Quaternion.identity);
        // _character = Instantiate(Resources.Load<GameObject>($"Prefabs/Unit/{캐릭터등급}/{캐릭터이름} ? "), worldPosition, Quaternion.identity);

        _character.transform.SetParent(this.transform);


        // 캐릭터 초기 위치 설정
        _previousPosition = _character.transform.position;
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
            SpawnCharacter();

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

                // 부모-자식 관계 및 위치 업데이트
                _character.transform.SetParent(transform);

                // RectTransform 위치를 사용하여 월드 좌표 계산
                Vector3 uiPosition = GetComponent<RectTransform>().position;
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(uiPosition.x, uiPosition.y, Camera.main.nearClipPlane));
                worldPosition.z = 0;
                _character.transform.position = worldPosition;

            }

            //현재 필드에 캐릭터가 있는 경우 - 교환
            else
            {
                // 캐릭터 교환
                GameObject tempCharacter = _character;
                _character = fromSlot._character;
                fromSlot._character = tempCharacter;

                // 각각의 부모-자식 관계 및 위치 업데이트
                _character.transform.SetParent(transform);
                Vector3 thisPosition = Camera.main.ScreenToWorldPoint(transform.position);
                thisPosition.z = 0;
                _character.transform.position = thisPosition;

                tempCharacter.transform.SetParent(fromSlot.transform);
                Vector3 otherPosition = Camera.main.ScreenToWorldPoint(fromSlot.transform.position);
                otherPosition.z = 0;
                tempCharacter.transform.position = otherPosition;
            }

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (_character != null)
        {
            // 유효한 위치에 드롭되지 않았다면 원위치
            CharacterPosReturn();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _fieldSlot.SelectSlot(this);
    }


    public void CharacterInit()
    {

        CharacterPosReturn();

    }
      

    private void CharacterPosReturn()
    {
        _character.transform.position = _previousPosition;

    }
}
