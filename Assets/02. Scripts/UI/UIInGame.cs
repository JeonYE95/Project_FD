using Assets.HeroEditor.Common.Scripts.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIBase
{
    [SerializeField] private Button drawBtn;
    [SerializeField] private Button unitGuideBtn;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _battleStartButton;
    [SerializeField] private Button combineInfoBtn;
    [SerializeField] private GameObject _spawnPointUI;

    [SerializeField] private Image mask1;
    [SerializeField] private Image mask2;
    [SerializeField] private Image mask3;
    
    private UIUnitGuide uiUnitGuide;
    private UICombineInfo uiCombineInfo;

    private void Start()
    {
        drawBtn.onClick.AddListener(() => { GachaManager.Instance.PlayGacha(); });  
        unitGuideBtn.onClick.AddListener(() => { OpenUnitGuideUI(); });  
        combineInfoBtn.onClick.AddListener(() => { OpenCombineInfoUI(); });  

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
    }

    private void OpenUnitGuideUI()
    {
        if (uiUnitGuide == null)
            uiUnitGuide = UIManager.Instance.GetUI<UIUnitGuide>();
        
        uiUnitGuide.Open();
    }

    private void OpenCombineInfoUI()
    {
        if (uiCombineInfo == null)
            uiCombineInfo = UIManager.Instance.GetUI<UICombineInfo>();
        
        uiCombineInfo.Open();
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
        _timerText.gameObject.SetActive(true);
    }
    
    private void SetStageHealth()
    {
        switch (StageManager.Instance.StageHealth)
        {
            case 3:
                break;
            case 2:
                mask3.SetActive(true);
                break;
            case 1:
                mask3.SetActive(true);
                mask2.SetActive(true);
                break;
            case 0:
                mask3.SetActive(true);
                mask2.SetActive(true);
                mask1.SetActive(true);
                break;
        }
    }

    // 스테이지 새로 시작할 때마다 호출
    public void InitializeStageHealth()
    {
        mask1.SetActive(false);
        mask2.SetActive(false);
        mask3.SetActive(false);
    }
}
