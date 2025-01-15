using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GSDatas;
using DG.Tweening;


public class UIUnitGuideSlot : UIBase
{

   [SerializeField] private Image _heroIcon;
   [SerializeField ]private Image _classIcon;

    private Button _findMixtureBtn;

    [SerializeField] private TextMeshProUGUI _heroName;

    private UnitData _unitData;


    private const string POOL_TAG = "UnitGuideSlot";
    private RectTransform _rectTransform;
    private float _startPosX;


    [SerializeField] private float showDuration = 2f;        // 나타나는 시간
    [SerializeField] private float stayDuration = 2f;          // 머무는 시간
    [SerializeField] private float hideDuration = 1f;        // 사라지는 시간


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

    }

    public void UpdateInfo(UnitData unitData)
    {

        _unitData = unitData;

        if (_heroName != null)
            _heroName.text = unitData.name;

        if (_heroIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Unit/UpperBody/{unitData.grade}/{unitData.name}");
            _heroIcon.sprite = sprite;
        }

        if (_classIcon != null)
        {
            Sprite sprite = Resources.Load<Sprite>($"Sprite/Icon/Class/{unitData.classtype}");
            _classIcon.sprite = sprite;
        }
    }

    public void UpdateInfo(UnitInfo unitData)
    {
        UpdateInfo(unitData._unitData);

    }

    public void PlaySlideAnimation()
    {
        // 이전 애니메이션 정지
        _rectTransform.DOKill();

        // 초기 위치 설정 (화면 상단 1/3 지점으로)
        _rectTransform.anchoredPosition = new Vector2(_rectTransform.rect.width, -130);

        // 애니메이션 시퀀스 생성
        Sequence sequence = DOTween.Sequence();

        // 왼쪽으로 슬라이드 인 (화면 안으로)
        sequence.Append(_rectTransform.DOAnchorPosX(0, showDuration)
            .SetEase(Ease.OutQuint));

        // 대기
        sequence.AppendInterval(stayDuration);

        // 오른쪽으로 슬라이드 아웃 (화면 밖으로)
        sequence.Append(_rectTransform.DOAnchorPosX(_rectTransform.rect.width, hideDuration)
            .SetEase(Ease.InQuint));


        // 애니메이션 완료 후 풀로 반환
        sequence.OnComplete(() => {
            ObjectPool.Instance.ReturnToPool(gameObject, POOL_TAG);
        });
    }

    private void OnDestroy()
    {
        // 애니메이션 정리
        _rectTransform?.DOKill();
    }


}
