using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillCoolTimeBar : MonoBehaviour
{
    Slider _skillCoolTimeSlider;
    ActionHandler _actionHandler;

    private void Awake()
    {
        // 캔버스에서 Slider 찾기
        _skillCoolTimeSlider = GetComponentInChildren<Slider>();
        if (_skillCoolTimeSlider == null)
        {
            Debug.LogError("UIHealthBar 프리팹에 스킬 Slider 없음");
            return;
        }
    }

    public void Initialize(ActionHandler actionHandler)
    {
        _actionHandler = actionHandler;
        _skillCoolTimeSlider.maxValue = _actionHandler.GetSkillCoolTimeMaxValue();
    }

    private void Update()
    {
        _skillCoolTimeSlider.value = _actionHandler.GetSkillCoolTimeValue();
    }
}
