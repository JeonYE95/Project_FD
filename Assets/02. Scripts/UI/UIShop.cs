using GSDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShop : UIBase
{
    public static UIShop Instance;
    [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _purchasePanel;
    [SerializeField] private TextMeshProUGUI _purchaseItemText;

    private int _curItemId;
    private bool _isFreeItem;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPurchase(int itemID, bool freeItem)
    {
        _curItemId = itemID;
        _isFreeItem = (itemID == 3000 || itemID == 3003);

        string itemName;

        if (freeItem)
        {
            itemName = RewardDataManager.Instance.GetUnitData(itemID).name;
            _purchaseItemText.text = $"{itemName}을(를) 받으시겠습니까?";
        }
        else
        {
            UnitData unit = UnitDataManager.Instance.GetUnitData(itemID);
            var (price, quantity) = GetUnitPieceData(unit.grade);
            _purchaseItemText.text = $"{unit.name}을(를) 구매하시겠습니까?";
        }

        // TODO : 구매창열기
    }

    public void ConfirmPurchase()
    {

    }

    public (int price, int quantity) GetUnitPieceData(string grade)
    {
        return grade switch
        {
            "Common" => (30000, 5),
            "Rare" => (60000, 3),
            "Unique" => (100000, 1),
            _ => (30000, 5)
        };
    }
}