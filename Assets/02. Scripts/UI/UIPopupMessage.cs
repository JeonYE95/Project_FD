using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UIPopupMessage : UIBase
{
    [SerializeField] private TMP_Text _message;
    [SerializeField] private Image _backgroundImage;

    private float _fadeInDuration = 0.5f;
    private float _displayDuration = 2f;
    private float _fadeOutDuration = 0.5f;

    public void ShowMessage(string message)
    {
        _message.text = message;
    
        // 팝업메세지 배경 초기값 설정
        Color backgroundColor = _backgroundImage.color;
        backgroundColor.a = 0f;
        _backgroundImage.color = backgroundColor;

        // 팝업메세지 텍스트 초기값 설정
        Color textColor = _message.color;
        textColor.a = 0f;
        _message.color = textColor;

        // fade in-out 시퀀스 실행
        Sequence fadeSequence = DOTween.Sequence();

        fadeSequence.Append(_backgroundImage.DOFade(1f, _fadeInDuration))
                    .Join(_message.DOFade(1f, _fadeInDuration))
                    .AppendInterval(_displayDuration)
                    .Append(_backgroundImage.DOFade(0f, _fadeOutDuration))
                    .Join(_message.DOFade(0f, _fadeOutDuration))
                    .OnComplete(() => { UIManager.Instance.CloseUI<UIPopupMessage>(); });

        fadeSequence.Play();
    }
}
