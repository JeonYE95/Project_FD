using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : UIBase
{
    [SerializeField] private Button drawBtn;
    [SerializeField] private Button unitGuideBtn;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private Button _battleStartButton;

    [SerializeField] private GameObject _spawnPointUI;

    private UIUnitGuide uiUnitGuide;

    private void Start()
    {
        drawBtn.onClick.AddListener(() => {  });    // 버튼 클릭 시 호출 함수 필요
        unitGuideBtn.onClick.AddListener(() => { OpenUnitGuideUI(); });  

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

    private void OpenUnitGuideUI()
    {
        if (uiUnitGuide == null)
            uiUnitGuide = UIManager.Instance.GetUI<UIUnitGuide>();
        
        uiUnitGuide.Open();
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
}
