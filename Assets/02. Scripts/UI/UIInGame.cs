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
    [SerializeField] private TMP_Text _stageNumText;
    
    [SerializeField] private Image _mask1;
    [SerializeField] private Image _mask2;
    [SerializeField] private Image _mask3;

    [SerializeField] private GameObject _gradeBtnParent;
    private Button[] _gradeButtons;

    [SerializeField] private Button _settingBtn;

    private Canvas _canvas;
    private Camera _mainCamera;

    private void Start()
    {
        // 카메라 할당
        _canvas = gameObject.GetComponent<Canvas>();
        _mainCamera = Camera.main;

        _canvas.worldCamera = _mainCamera;

        SetupUI();

        _drawBtn.onClick.AddListener(() => { IngameGacha.Instance.PlayGacha(); });  
        _unitGuideBtn.onClick.AddListener(() => { UIManager.Instance.GetUI<UIUnitGuide>(); });
        _settingBtn.onClick.AddListener(() => { UIManager.Instance.OpenUI<UIInGameSetting>(); });

        //WaveManager 버튼 연동
        _battleStartButton.onClick.AddListener(WaveManager.Instance.WaveStartNow);
        //타이머 연동
        WaveManager.Instance.OnPreparationTimeChanged += UpdateTimerText;

        // 전투 시작시 UI 비활성화 
        WaveManager.Instance.OnBattleStart += DisablePrepUI;

        //전투 종료시 UI 활성화
        WaveManager.Instance.OnClearWave += EnablePrepUI;

        // 등급 버튼 연결
        _gradeButtons = _gradeBtnParent.GetComponentsInChildren<Button>();
        Button commonButton = _gradeButtons[(int)Defines.UnitGrade.common];
        Button rareButton = _gradeButtons[(int)Defines.UnitGrade.rare];
        Button uniqueButton = _gradeButtons[(int)Defines.UnitGrade.Unique];

        commonButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.common, commonButton));
        rareButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.rare, rareButton));
        uniqueButton.onClick.AddListener(() => OnGradeButtonClick(Defines.UnitGrade.Unique, uniqueButton));

        OnGradeButtonClick(Defines.UnitGrade.common, commonButton);
    }

    private void Update()
    {
        SetStageHealth();   // 게임 종료 시마다(성공 또는 실패 시 마다) 호출하는게 더 나으려나
        SetGold();
        SetUnitLimit();
        SetStageNum();
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

    private void SetStageNum()
    {
        _stageNumText.text = $"1-{GameManager.Instance.StageID % 10}";
    }

    private void OnGradeButtonClick(Defines.UnitGrade units, Button clickedButton)
    {
        InventoryManager.Instance.UpdateUnitGrade(units);

        foreach (Button btn in _gradeButtons)
        {
            btn.GetComponent<Image>().color = btn.colors.normalColor;
        }

        clickedButton.GetComponent<Image>().color = new Color(255f / 255f, 210f / 255f, 0f);
    }


    void SetupUI()
    {
        // 화면 비율 계산
        float screenRatio = (float)Screen.width / Screen.height;
        float targetRatio = 16f / 9f;

        if (screenRatio > targetRatio)
        {
            RectTransform spawnPointRect = _spawnPointUI.GetComponent<RectTransform>();
            Vector3 localScale = spawnPointRect.localScale;

            // 16:9 비율을 기준으로 현재 화면 비율에 따라 스케일 조정
            float ratioDiff = screenRatio - targetRatio;
            float scale = 1.3f - (ratioDiff * 1.14f);  // 1.3은 초기 scale값, 1.14는 (16 : 9 기준으로 20 : 9 까지 비율 계산)

            localScale.y = scale;
            spawnPointRect.localScale = localScale;
        }
    }

}
