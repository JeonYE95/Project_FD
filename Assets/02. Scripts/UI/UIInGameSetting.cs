using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIInGameSetting : UIBase
{

    private Coroutine savedCoroutine;

    [SerializeField] private Button _exitBtn;
    [SerializeField] private Slider _bgmSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Button _homeBtn;

    private float savedTime;

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

        _exitBtn.onClick.AddListener(() => { UIManager.Instance.CloseUI<UIInGameSetting>(); });
        _homeBtn.onClick.AddListener(() => { LoadMainScene(); });

    }

    private void SetBGMVolume(float value)
    {
        SoundManager.Instance.SetBGMVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        SoundManager.Instance.SetSFXVolume(value);
    }


    private void LoadMainScene()
    {
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                UIManager.Instance.OpenUI<UIMain>();
       
            }
        };

        UIManager.Instance.Clear();
        SceneManager.LoadScene("MainScene");
        SoundManager.Instance.PlayBGM("MainBGM");
    }

    private void OnEnable()
    {
        StageManager.Instance.StopGame();
    }

    public void OnDisable()
    {
        StageManager.Instance.ResumeGame();

    }

}
