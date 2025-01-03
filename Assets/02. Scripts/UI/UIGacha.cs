using System.Collections;
using System.Collections.Generic;
using Art_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGacha : UIBase
{
    [SerializeField] private TMP_Text _diamondTxt;

    [SerializeField] private Button _gachaBtn1;
    [SerializeField] private Button _gachaBtn10;

    private PieceGacha pieceGacha = new PieceGacha();

    // Start is called before the first frame update
    void Start()
    {
        
        _gachaBtn1.onClick.AddListener(() => { pieceGacha.PlayPieceGacha(); } );
        _gachaBtn1.onClick.AddListener(() => { pieceGacha.PlayPieceGacha(); } );
    }

    // Update is called once per frame
    void Update()
    {
        _diamondTxt.text = GameManager.Instance.playerData.diamond.ToString();
    }
}
