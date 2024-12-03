
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _previousPosition;

    private GameObject _previewObject;
    private Image _draggedCharacterPreview;

    private Inventory _characterSlot;
    public int Index { get; private set; }

    public void Init(Inventory characterSlot, int index)
    {
        _characterSlot = characterSlot;
        Index = index;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _previousPosition = transform;

        if (_previewObject == null)
        {
            _previewObject = new GameObject("DragPreview");
            _draggedCharacterPreview = _previewObject.AddComponent<Image>();


            // 테스트 용 , 실제는 캐릭터 ID 받아서 DB 저장 경로로 불러오기 
            _draggedCharacterPreview.sprite = Resources.Load<Sprite>("Textures/Test1");


            var canvas = FindObjectOfType<Canvas>();
            _previewObject.transform.SetParent(canvas.transform);

            _draggedCharacterPreview.raycastTarget = false;
            _previewObject.transform.SetAsLastSibling();

        }


        //DB에 저장된 캐릭터 ID로 불러오기 
        //draggedCharacterPreview.sprite = Resources.Load<Sprite>("Textures/캐릭터이름");

        _previewObject.SetActive(true);
        _previewObject.transform.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_previewObject != null && _previewObject.activeSelf)
        {
            _previewObject.transform.position = eventData.position;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // SetActive , 재활용
        if (_previewObject != null)
        {
            _previewObject.SetActive(false);
        }

    }

}
