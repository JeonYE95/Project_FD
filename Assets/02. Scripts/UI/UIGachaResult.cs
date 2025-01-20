using GSDatas;
using UnityEngine;
using UnityEngine.UI;

public class UIGachaResult : UIBase
{
    [SerializeField] private Button _checkBtn;
    [SerializeField] private RectTransform _pieceParent;
    [SerializeField] private GameObject _piecePrefab;
    private UIPiece[] _pieces;

    private void Awake() 
    {

    }

    void Start()
    {
        _checkBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIGachaResult>(); }); 
    }

    public void ShowGachaResult(GachaData[] gachaData)
    {
        DestroyPreviousPieces();

        _pieces = new UIPiece[gachaData.Length];

        for (int i = 0; i < gachaData.Length; i++)
        {
            GameObject pieceObj = Instantiate(_piecePrefab, _pieceParent);
            pieceObj.name = $"Piece_{i+1}";

            _pieces[i] = pieceObj.GetComponent<UIPiece>();

            _pieces[i].SetPieceInfo(gachaData[i]);
        }
    }

    private void DestroyPreviousPieces()
    {
        foreach (RectTransform child in _pieceParent)
        {
            Destroy(child.gameObject);
        }
    }
}
