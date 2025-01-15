using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGacha : UIBase
{
    [SerializeField] private TMP_Text _diamondTxt;
    [SerializeField] private Image _unitImg;

    [SerializeField] private Button _gachaBtn1;
    [SerializeField] private Button _gachaBtn10;
    [SerializeField] private Button _backBtn;

    [SerializeField] private Image _gachaResult;
    [SerializeField] private TMP_Text _resultText;

    private float _fadeInDuration = 0.5f;
    private float _displayDuration = 2f; 
    private float _fadeOutDuration = 0.5f; 

    private PieceGacha pieceGacha = new PieceGacha();

    void Start()
    {
        _gachaBtn1.onClick.AddListener(() => { pieceGacha.PlayPieceGacha(); ShowResult(pieceGacha.Result); });
        _gachaBtn10.onClick.AddListener(() => { pieceGacha.PlayPieceGacha10(); });
        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIGacha>(); });
    }

    void Update()
    {
        _diamondTxt.text = GameManager.Instance.playerData.diamond.ToString();
    }

    public void ShowResult(string resultMessage)
    {
        if (_gachaResult != null && _resultText != null)
        {
            // 이미지와 텍스트 초기 상태 설정
            Color imageColor = _gachaResult.color;
            imageColor.a = 0f;
            _gachaResult.color = imageColor;

            _resultText.text = resultMessage; 
            Color textColor = _resultText.color;
            textColor.a = 0f;
            _resultText.color = textColor;

            Sequence fadeSequence = DOTween.Sequence();

            fadeSequence.Append(_gachaResult.DOFade(1f, _fadeInDuration))
                        .Join(_resultText.DOFade(1f, _fadeInDuration)) 
                        .AppendInterval(_displayDuration) 
                        .Append(_gachaResult.DOFade(0f, _fadeOutDuration)) 
                        .Join(_resultText.DOFade(0f, _fadeOutDuration)); 

            fadeSequence.Play();
        }

        _unitImg.sprite = Resources.Load<Sprite>($"Sprite/Unit/WholeBody/{pieceGacha.SelectedUnit.grade}/{pieceGacha.SelectedUnit.name}");
    }

    private void OnEnable() 
    {
        _unitImg.sprite = Resources.Load<Sprite>("Sprite/none_");
    }
}
