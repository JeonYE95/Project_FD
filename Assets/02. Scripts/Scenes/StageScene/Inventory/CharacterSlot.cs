
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CharacterSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform _previousPosition;

    [SerializeField]
    private Image _draggedCharacterPreview;
    [SerializeField]
    private GameObject _previewObject;
    [SerializeField]
    private UIUnitSlot _UIUnitSlot;


    // 테스트 용
    [SerializeField]
    private UIUnitSlotTest _unitSlotTest;


    public int Index { get; private set; }

    private void Start()
    {
        //자신의 순서를 인덱스로 자동 할당
        Index = transform.GetSiblingIndex();

        if (InventoryManager.Instance.PreviewObject == null)
        {
            CreatePreviewObject();
        }
        else
        {
            _previewObject = InventoryManager.Instance.PreviewObject;
            _draggedCharacterPreview = _previewObject.GetComponent<Image>();
        }

        _unitSlotTest = GetComponentInParent<UIUnitSlotTest>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _previousPosition = transform;

        if (_previewObject != null)
        {


            // 실제 동작 코드
            /*
             
              // 현재 슬롯 인덱스에 해당하는 유닛 정보 가져오기
            if (_UIUnitSlot != null && Index < _UIUnitSlot.InventoryUnits.Count)
            {
                Unit currentUnit = _UIUnitSlot.InventoryUnits[Index];
                if (currentUnit != null)
                {
                  
                    // 유닛 정보에 맞는 이미지로 업데이트
                    _draggedCharacterPreview.sprite = Resources.Load<Sprite>($"Sprite/{currentUnit.Name}");


                    UnitPrevInfo previewInfo = _previewObject.GetComponent<UnitPrevInfo>();
                    previewInfo.SetUnitInfo(currentUnit);
                }

            }
             
             */



            //테스트 코드 
            {

                // 현재 슬롯 인덱스에 해당하는 유닛 정보 가져오기
                if (_unitSlotTest != null && Index < _unitSlotTest.InventoryUnits.Count)
            {
                UnitInfo currentUnit = _unitSlotTest.InventoryUnits[Index];
                if (currentUnit != null)
                {
                  
                    // 유닛 정보에 맞는 이미지로 업데이트
                    _draggedCharacterPreview.sprite = Resources.Load<Sprite>($"Sprite/{currentUnit._unitData.name}");


                    UnitPrevInfo previewInfo = _previewObject.GetComponent<UnitPrevInfo>();
                    previewInfo.SetUnitInfo(currentUnit);
                }

            }
            
            }


            _previewObject.SetActive(true);
            _previewObject.transform.position = eventData.position;

        }
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
        // preview 이미지 재활용
        if (_previewObject != null)
        {
            _previewObject.SetActive(false);
        }

    }


    private void CreatePreviewObject()
    {
        _previewObject = new GameObject("DragPreview");
        _draggedCharacterPreview = _previewObject.AddComponent<Image>();
        _draggedCharacterPreview.raycastTarget = false;

        
        UnitPrevInfo previewInfo = _previewObject.AddComponent<UnitPrevInfo>();

        var canvas = FindObjectOfType<Canvas>();
        _previewObject.transform.SetParent(canvas.transform);
        _previewObject.transform.SetAsLastSibling();

        InventoryManager.Instance.PreviewObject = _previewObject;
        _previewObject.SetActive(false);
    }

}
