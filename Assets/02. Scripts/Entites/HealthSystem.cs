using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    private GameObject _healthBar;
    private RectTransform _healthBarRectTransform;

    private int maxHP;
    public int currentHP;

    //현재는 최대 HP가 바뀌면 현재 HP도 최대 HP로 설정
    //배틀 시 최대 체력이 바뀔리 없다는 가정
    public int MaxHP
    {
        get 
        { 
            return maxHP;
        }
        set 
        {
            maxHP = value;
            currentHP = maxHP;
        }
    }

    BaseUnit unit;
    private void Awake()
    {
        unit = GetComponent<BaseUnit>();
    }

    private void Start()
    {
        CreateHealthBar();
    }

    private void CreateHealthBar()
    {
        GameObject healthBarPrefab = Resources.Load<GameObject>("UI/UIPlayerHealthBar");
        if (healthBarPrefab == null)
        {
            Debug.LogError("HealthBar 프리팹을 Resources/Prefabs/UIHealthBar에서 찾을 수 없습니다!");
            return;
        }

        // HealthBar 인스턴스 생성
        _healthBar = Instantiate(healthBarPrefab, transform);

        // UIHealthBar 컴포넌트 설정
        UIHealthBar healthBarScript = _healthBar.GetComponent<UIHealthBar>();
        if (healthBarScript != null)
        {
            healthBarScript.Initialize(this); // HealthSystem 연결
        }
        else
        {
            Debug.Log("스크립트연결해야함");
        }

        // HealthBar 위치 조정 
        _healthBarRectTransform = _healthBar.GetComponent<RectTransform>();

        UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        // 캐릭터의 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2.0f, 0)); // 머리 위로 오프셋
        Debug.Log(transform.position);

        // 스크린 좌표를 HealthBar의 anchoredPosition에 적용
        _healthBarRectTransform.localPosition = screenPos;

        // Z값이 음수면 카메라 뒤쪽에 있으므로 HealthBar를 비활성화
        if (screenPos.z < 0)
        {
            _healthBar.SetActive(false);
        }
        else
        {
            _healthBar.SetActive(true);
        }
    }

    public void ResetHealth()
    {
        currentHP = maxHP;
    }

    public void TakeHealth(int heal)
    {
        currentHP = Mathf.Min(currentHP + heal, maxHP);
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP < 0)
        {
            unit.CallDieEvent();
        }
    }
}
