using GSDatas;
using System;
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

    private const string shopResetKey = "ShopResetTime";
    private const string freeEnergyKey = "FreeEnergy";
    private const string freeDiamondKey = "FreeDiamond";

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        CheckDailyReset();
    }

    // 자정 기준 데일리 리셋 체크
    private void CheckDailyReset()
    {
        if (!PlayerPrefs.HasKey(shopResetKey))
        {
            PlayerPrefs.SetString(shopResetKey, System.DateTime.Now.ToString());
            PlayerPrefs.Save();
            return;
        }

        string lastResetTime = PlayerPrefs.GetString(shopResetKey);
        System.DateTime lastReset = System.DateTime.Parse(lastResetTime);

        if (System.DateTime.Now.Date > lastReset.Date) 
        {
            ResetShop();
        }
    }

    // 상점 리셋
    private void ResetShop()
    {
        PlayerPrefs.SetString(freeEnergyKey, "");
        PlayerPrefs.SetString(freeDiamondKey, "");
        PlayerPrefs.SetString(shopResetKey, System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    // 무료 보상 획득 가능 여부
    public bool CanGetFreeReward(int itemId)
    {
        string key = itemId == 3000 ? freeEnergyKey : freeDiamondKey;
        if (!PlayerPrefs.HasKey(key) || string.IsNullOrEmpty(PlayerPrefs.GetString(key)))
        {
            return true;
        }

        string lastGetTimeString = PlayerPrefs.GetString(key);
        System.DateTime lastGetTime = System.DateTime.Parse(lastGetTimeString);
        return (System.DateTime.Now.Date > lastGetTime.Date);
    }

    // 무료 보상 시간 저장
    private void SaveFreeRewardTime(int itemId)
    {
        string key = itemId == 3000 ? freeEnergyKey : freeDiamondKey;
        PlayerPrefs.SetString(key, System.DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    // 구매 확인창 오픈
    public void ShowPurchaseConfirmation(int itemId)
    {
        _curItemId = itemId;
        _isFreeItem = (itemId == 3000 || itemId == 3003);

        if (_isFreeItem)
        {
            bool canBuy = CanGetFreeReward(itemId);
            RewardData reward = RewardDataManager.Instance.GetUnitData(itemId);
            _purchaseItemText.text = canBuy ? $"{reward.name}을(를) 무료로 구매하시겠습니까?" : "이미 무료 보상을 수령했습니다.";
        }
        else
        {
            UnitData unit = UnitDataManager.Instance.GetUnitData(itemId);
            var (price, quantity) = GetUnitPieceData(unit.grade);
            _purchaseItemText.text = $"{unit.name}을 {price} 골드에 구매하시겠습니까?";
        }

        // TODO : 구매창 열기
    }


    // 구매 로직
    public void ShowPurchase()
    {
        if (_isFreeItem)
        {
            if (CanGetFreeReward(_curItemId))
            {
                if (_curItemId == 3000)
                {
                    GameManager.Instance.playerData.energy += 5;
                }
                else if (_curItemId == 3003)
                {
                    GameManager.Instance.playerData.diamond += 200;
                }

                SaveFreeRewardTime(_curItemId);
            }
        }
        else
        {
            UnitData unit = UnitDataManager.Instance.GetUnitData(_curItemId);
            var (price, quantity) = GetUnitPieceData(unit.grade);

            if (GameManager.Instance.playerData.gold >= price)
            {
                GameManager.Instance.playerData.gold -= price;
                GameManager.Instance.AddItemSave(_curItemId, quantity);
            }
        }

        GameManager.Instance.SavePlayerDataToJson();
        // TODO : 구매창열기
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