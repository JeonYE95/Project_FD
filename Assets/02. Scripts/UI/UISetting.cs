using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIBase
{
    [SerializeField] private Button _exitBtn;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    
    void Start()
    {
        if (_bgmSlider != null)
        {
            _bgmSlider.value = SoundManager.Instance.GetBGMVolume();
            _bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.value = SoundManager.Instance.GetSFXVolume();
            _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        _exitBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UISetting>(); });
    }

    private void SetBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }
}
