using UnityEngine;
using UnityEngine.UI;

public class UIStageInfo : MonoBehaviour
{
    private Slider _stageSlider;
    private int _currentWave;

    void Awake()
    {
        _stageSlider = GetComponentInChildren<Slider>();

        if (_stageSlider == null)
            Debug.LogError("StageInfo 오브젝트에 Slider 없음");
    }

    private void Start() 
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveChanged += UpdateStageSliderValue;
    }

    private void OnDestroy()
    {
        if (WaveManager.Instance != null)
            WaveManager.Instance.OnWaveChanged -= UpdateStageSliderValue;
    }

    private void UpdateStageSliderValue(int currentWave)
    {
        _currentWave = currentWave;
        
        switch(_currentWave)
        {
            case 1:
                _stageSlider.value = 0f;
                break;
            case 2:
                _stageSlider.value = 0.25f;
                break;
            case 3:
                _stageSlider.value = 0.5f;
                break;
            case 4:
                _stageSlider.value = 0.75f;
                break;
            // 보스
            case 5: 
                _stageSlider.value = 1f;
                break;
            default: 
                Debug.Log("웨이브 값 오류");
                break;
        }
    }
}
