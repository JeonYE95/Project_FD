using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGameTest : UIBase
{
    private TMP_Text _timerText;
    private Button _battleStartButton;

    public void Start()
    {
        _battleStartButton = GetComponentInChildren<Button>();
        _timerText = GetComponentInChildren<TMP_Text>();

        //WaveManager 버튼 연동
        _battleStartButton.onClick.AddListener(WaveManager.Instance.WaveStartNow);
        //타이머 연동
        WaveManager.Instance.OnPreparationTimeChanged += UpdateTimerText;
    }

    private void UpdateTimerText(float remainingTime)
    {
        _timerText.text = WaveManager.Instance.GetCurrentPreparationTimeText();
    }

}
