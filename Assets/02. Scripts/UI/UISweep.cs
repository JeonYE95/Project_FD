using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GSDatas;

public class UISweep : UIBase
{
    [SerializeField] private Button _closeBtn;
    [SerializeField] private Button _sweepClearBtn;
    [SerializeField] private Button _plusBtn;
    [SerializeField] private Button _minusBtn;

    [SerializeField] private Button _maxBtn;
    [SerializeField] private Button _minBtn;
    [SerializeField] private TextMeshProUGUI _haveEnergyTxt;
    [SerializeField] private TextMeshProUGUI _consumeEnergyTxt;
    [SerializeField] private TextMeshProUGUI _sweepCount;
    
    private List<StageData> _currentStageData;

    private int _stageEnergyCost;
    private int _consumeEnergy = 1;
    private int _canSweepCount = 1;

    public void SetStageData(List<StageData> stageData)
    {
        _currentStageData = stageData;

        _stageEnergyCost = _currentStageData[0].cost;
        _consumeEnergy = _stageEnergyCost;
        _canSweepCount = 1;
        _sweepCount.text = "1";

        UpdateEnergyText();
        _haveEnergyTxt.text = GameManager.Instance.playerData.energy.ToString();

        CheckEnergy();
    }

 

    private void Start()
    {
     
        _closeBtn.onClick.AddListener(() => { Close(); });
        _sweepClearBtn.onClick.AddListener(OnSweepClearClick);
        _plusBtn.onClick.AddListener(IncreaseEnergy);
        _minusBtn.onClick.AddListener(DecreaseEnergy); 
        _maxBtn.onClick.AddListener(SetMaxEnergy);
        _minBtn.onClick.AddListener(SetMinEnergy);
    }


    private void IncreaseEnergy()
    {
        // 다음 소탕에 필요한 에너지가 보유 에너지보다 작거나 같은 경우
        if (_consumeEnergy + _stageEnergyCost <= GameManager.Instance.playerData.energy)
        {
            _canSweepCount++;
            _consumeEnergy += _stageEnergyCost;
            UpdateEnergyText();
        }
    }

    private void DecreaseEnergy()
    {
        if (_canSweepCount > 1)
        {
            _canSweepCount--;
            _consumeEnergy -= _stageEnergyCost;
            UpdateEnergyText();
        }
    }

    private void SetMaxEnergy()
    {
        int maxSweepCount = GameManager.Instance.playerData.energy / _stageEnergyCost;
        _canSweepCount = maxSweepCount;
        _consumeEnergy = _stageEnergyCost * maxSweepCount;
        UpdateEnergyText();
    }

    private void SetMinEnergy()
    {
        _canSweepCount = 1;
        _consumeEnergy = _stageEnergyCost;
        UpdateEnergyText();
    }

    private void UpdateEnergyText()
    {
        _consumeEnergyTxt.text = _consumeEnergy.ToString();
        _sweepCount.text = _canSweepCount.ToString();
    }

    private void CheckEnergy()
    {
        _sweepClearBtn.interactable = _consumeEnergy <= GameManager.Instance.playerData.energy;
    }
    private void OnSweepClearClick()
    {
        GameObject clearUI = Instantiate(Resources.Load<GameObject>("UI/UISweepClear")); // 경로는 실제 프리팹 위치에 맞게 수정
        UISweepClear sweepClear = clearUI.GetComponent<UISweepClear>();
        sweepClear.SweepCount(_consumeEnergy);
        sweepClear.SetStageData(_currentStageData);
        sweepClear.Open();

        GameManager.Instance.EnterEnergy -= _consumeEnergy;
        QuestManager.Instance.UpdateConsumeQuests(3000, _consumeEnergy);

        Close(); // 현재 UI 닫기

    }

}
