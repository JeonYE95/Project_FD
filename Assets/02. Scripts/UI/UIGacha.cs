using System.Collections.Generic;
using DG.Tweening;
using GSDatas;
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

    private PieceGacha pieceGacha = new PieceGacha();
    private int _gachaCount;

    private GachaData[] _gachaData1 = new GachaData[1];
    private GachaData[] _gachaData10 = new GachaData[10];

    void Start()
    {
        _gachaBtn1.onClick.AddListener(() => 
        { 
            _gachaCount = 1;
            _gachaData1[0] = pieceGacha.PlayPieceGacha(); 
            ShowResult(); 
        });
        _gachaBtn10.onClick.AddListener(() => 
        { 
            _gachaCount = 10;
            _gachaData10 = pieceGacha.PlayPieceGacha10(); 
            ShowResult(); 
        });
        _backBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIGacha>(); });
    }

    void Update()
    {
        _diamondTxt.text = GameManager.Instance.playerData.diamond.ToString();
    }

    public void ShowResult()
    {   
        // 다이아가 부족한 경우
        if (!pieceGacha.IsEnoughDiamonds)
        {
            UIManager.Instance.OpenUI<UIPopupMessage>().ShowMessage("다이아가 부족합니다.");
        }
        // 다이아가 충분한 경우
        else
        {
            UIGachaResult uIGachaResult= UIManager.Instance.OpenUI<UIGachaResult>();

            if (_gachaCount == 1)
                uIGachaResult.ShowGachaResult(_gachaData1);
            else if (_gachaCount == 10)
                uIGachaResult.ShowGachaResult(_gachaData10);
        }
    }

    private void OnEnable() 
    {
        _unitImg.sprite = Resources.Load<Sprite>("Sprite/none_");
    }
}
