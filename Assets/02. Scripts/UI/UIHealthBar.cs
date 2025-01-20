using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : UIBase
{
    private Slider _healthSlider;
    private HealthSystem _healthSystem;
    private GameObject _healthBar;

    public void Initialize(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;

        if (_healthSlider != null)
        {
            _healthSlider.maxValue = _healthSystem.MaxHP;
            _healthSlider.value = _healthSystem.currentHP;
        }
    }
    
    private void Awake()
    {
        // PlayerBasePrefab에서 HealthSystem 찾기
        _healthSystem = GetComponentInParent<HealthSystem>();
        if (_healthSystem == null)
        {
            Debug.LogError("부모 객체에 HealthSystem 없음");
            return;
        }

        // 캔버스에서 Slider 찾기
        _healthSlider = GetComponentInChildren<Slider>();
        if (_healthSlider == null)
        {
            Debug.LogError("UIHealthBar 프리팹에 Slider 없음");
            return;
        }
    }

    private void Start()
    {
        _healthSlider.maxValue = _healthSystem.MaxHP;
        _healthSlider.value = _healthSystem.currentHP;
        _healthSystem.OnHealthChange += SetHealthBar;
    }

    public void SetHealthBar()
    {
        if (_healthSlider != null)
        {
            _healthSlider.value = _healthSystem.currentHP;
        }
    }
}
