using GSDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPiece : MonoBehaviour
{
    [SerializeField] private TMP_Text _pieceCount;
    [SerializeField] private Image _unitImg;


    void Update()
    {
        
    }

    private void OnEnable() 
    {
        
    }

    public void SetPieceInfo(GachaData gachaData)
    {
        // _pieceCount, _unitImg 설정
        _pieceCount.text = gachaData.pieceamount.ToString();
        _unitImg.sprite = Resources.Load<Sprite>($"Sprite/Unit/UpperBody/{gachaData.grade}/{gachaData.name}");

    }

}
