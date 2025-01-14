using GSDatas;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopSlot : UIBase
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemText;
    [SerializeField] private TextMeshProUGUI _itemPrice;

    private int _itemId;
    private bool isFreeItem;

    // 아이템 세팅
    private void SetItem(int itemId)
    {
        this._itemId = itemId;
        isFreeItem = (itemId == 3000 || itemId == 3003);

        if (isFreeItem)
        {
            RewardData reward = RewardDataManager.Instance.GetUnitData(itemId);
            bool canBuy = UIShop.Instance.CanGetFreeReward(itemId);
            _itemPrice.text = reward.name;
            //_itemIcon.sprite = 
            _itemPrice.text = canBuy ? "FREE" : "24시간 후 재구매 가능";
        }
        else
        {
            UnitData unit = UnitDataManager.Instance.GetUnitData(itemId);
            _itemText.text = unit.name;
            //_itemIcon.sprite =
            var (price, quantity) = UIShop.Instance.GetUnitPieceData(unit.grade);
            _itemPrice.text = $"{price} G";
        }

        // TODO : 구매창 오픈
    }
}
