using Assets.HeroEditor.Common.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIBase
{
    [SerializeField] private Button _drawBtn;
    [SerializeField] private Button _unitGuideBtn;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _battleStartButton;
    [SerializeField] private GameObject _spawnPointUI;
    [SerializeField] private TMP_Text _gold;
    [SerializeField] private TMP_Text _currentSummonUnitNum;
    [SerializeField] private TMP_Text _maxSummonUnitNum;

    [SerializeField] private Image _mask1;
    [SerializeField] private Image _mask2;
    [SerializeField] private Image _mask3;

    private Canvas _canvas;
    private Camera _mainCamera;

    private void Start()
    {
        // 카메라 할당
        _canvas = gameObject.GetComponent<Canvas>();
        _mainCamera = Camera.main;

        _canvas.worldCamera = _mainCamera;

        _drawBtn.onClick.AddListener(() => { IngameGacha.Instance.PlayGacha(); });  
        _unitGuideBtn.onClick.AddListener(() => { UIManager.Instance.GetUI<UIUnitGuide>(); });  

        // _battleStartButton = GetComponentInChildren<Button>();
        // _timerText = GetComponentInChildren<TMP_Text>();

        //WaveManager 버튼 연동
        _battleStartButton.onClick.AddListener(WaveManager.Instance.WaveStartNow);
        //타이머 연동
        WaveManager.Instance.OnPreparationTimeChanged += UpdateTimerText;

        // 전투 시작시 UI 비활성화 
        WaveManager.Instance.OnBattleStart += DisablePrepUI;

        //전투 종료시 UI 활성화
        WaveManager.Instance.OnClearWave += EnablePrepUI;
    }

    private void Update()
    {
        SetStageHealth();   // 게임 종료 시마다(성공 또는 실패 시 마다) 호출하는게 더 나으려나
        SetGold();
        SetUnitLimit();
    }

    private void UpdateTimerText(float remainingTime)
    {
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        _timerText.text = $"{seconds:00}";
    }

    private void DisablePrepUI()
    {
        _battleStartButton.gameObject.SetActive(false);
        _spawnPointUI.gameObject.SetActive(false);
    }

    private void EnablePrepUI()
    {
        _battleStartButton.gameObject.SetActive(true);
        _spawnPointUI.gameObject.SetActive(true);
       
    }
    
    private void SetStageHealth()
    {
        switch (StageManager.Instance.StageHealth)
        {
            case 3:
                _mask1.SetActive(false);
                _mask2.SetActive(false);
                _mask3.SetActive(false);
                break;
            case 2:
                _mask1.SetActive(false);
                _mask2.SetActive(false);
                _mask3.SetActive(true);
                break;
            case 1:
                _mask1.SetActive(false);
                _mask2.SetActive(true);
                _mask3.SetActive(true);
                break;
            case 0:
                _mask1.SetActive(true);
                _mask2.SetActive(true);
                _mask3.SetActive(true);
                break;
        }
    }

    private void SetGold()
    {
        _gold.text = StageManager.Instance.Gold.ToString();
    }

    private void SetUnitLimit()
    {
        _currentSummonUnitNum.text = InventoryManager.Instance.SummonUnitCount.ToString();
        _maxSummonUnitNum.text = InventoryManager.Instance.MaxSummonUnitCount.ToString();
    }

    
}
