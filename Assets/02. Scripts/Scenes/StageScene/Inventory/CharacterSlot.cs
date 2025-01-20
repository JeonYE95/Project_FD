
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GSDatas;
public class CharacterSlot : Slot, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Transform _previousPosition;
    private Canvas _canvas; // Canvas 참조 캐싱용 변수 추가

    [SerializeField]
    private Image _draggedCharacterPreview;
    [SerializeField]
    private GameObject _previewObject;
    [SerializeField]
    private UIUnitSlot _uiUnitSlot;
    private UICombineInfo _uiCombineInfo;


    [SerializeField] private int _index;
    public int Index => _index;  // 읽기 전용 프로퍼티

    public void SetIndex(int newIndex)
    {
        _index = newIndex;
    }

    private void Start()
    {

        _uiUnitSlot = GetComponentInParent<UIUnitSlot>();
        _canvas = GetComponentInParent<Canvas>(); // Start에서 한 번만 가져오기


        if (InventoryManager.Instance.PreviewObject == null)
        {
            CreatePreviewObject();
        }
        else
        {
            _previewObject = InventoryManager.Instance.PreviewObject;
            _draggedCharacterPreview = _previewObject.GetComponent<Image>();
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _previousPosition = transform;

        if (_previewObject != null)
        {

            // 현재 슬롯 인덱스에 해당하는 유닛 정보 가져오기
            if (_uiUnitSlot != null)
            {
                UnitData currentUnit = _uiUnitSlot.GetUnitAtIndex(Index);
                if (currentUnit != null)
                {

                    // 유닛 정보에 맞는 이미지로 업데이트
                    _draggedCharacterPreview.sprite = Resources.Load<Sprite>($"Sprite/Unit/WholeBody/{currentUnit.grade}/{currentUnit.name}");


                    UnitPrevInfo previewInfo = _previewObject.GetComponent<UnitPrevInfo>();
                    previewInfo.SetUnitData(currentUnit);
                }

            }


            _previewObject.SetActive(true);

            //캔버스 UI 공간 좌표에 맞게 변환
            _previewObject.transform.position = _canvas.GetMouseWorldPosition(eventData.position);

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_previewObject != null && _previewObject.activeSelf)
        {
            // 캔버스 공간으로 변환
            _previewObject.transform.position = _canvas.GetMouseWorldPosition(eventData.position);
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // preview 재활용
        if (_previewObject != null)
        {

            // 유닛 정보 초기화
            UnitPrevInfo previewInfo = _previewObject.GetComponent<UnitPrevInfo>();
            if (previewInfo != null)
            {
                previewInfo.SetUnitData(null); // null을 전달하여 정보 초기화
            }

            // 이미지도 초기화
            _draggedCharacterPreview.sprite = null;

            _previewObject.SetActive(false);
        }

    }

    private void CreatePreviewObject()
    {
        _previewObject = new GameObject("DragPreview");
        _draggedCharacterPreview = _previewObject.AddComponent<Image>();
        _draggedCharacterPreview.raycastTarget = false;

        RectTransform rectTransform = _previewObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(100, 100);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        UnitPrevInfo previewInfo = _previewObject.AddComponent<UnitPrevInfo>();

        // 캐싱된 Canvas 사용
        _previewObject.transform.SetParent(_canvas.transform, false);
        _previewObject.transform.SetAsLastSibling();

        InventoryManager.Instance.PreviewObject = _previewObject;
        _previewObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_uiUnitSlot != null)
        {
            UnitData clickedUnit = _uiUnitSlot.GetUnitAtIndex(Index); // 현재 슬롯에 있는 유닛 데이터 가져오기

            // UICombineInfo를 활성화하고 클릭한 유닛의 ID 전달
            if (clickedUnit != null)
            {
                if (clickedUnit.grade == "Common")
                {
                    UIManager.Instance.CloseUI<UICombineInfo_4>();
                    UIManager.Instance.OpenUI<UICombineInfo>().OnUnitClicked(clickedUnit.ID);
                }
                else if (clickedUnit.grade == "Rare") 
                {
                    UIManager.Instance.CloseUI<UICombineInfo>();
                    UIManager.Instance.OpenUI<UICombineInfo_4>().OnUnitClicked(clickedUnit.ID);
                }
                else
                {
                    Debug.Log("조합식 없음");
                }
            }
            else
            {
                Debug.LogWarning("슬롯에 유닛 데이터가 없습니다.");
            }
        }
    }

    private void OpenCombineInfoUI()
    {
        if (_uiCombineInfo == null)
            _uiCombineInfo = UIManager.Instance.GetUI<UICombineInfo>();
        
        _uiCombineInfo.Open();
    }

}
