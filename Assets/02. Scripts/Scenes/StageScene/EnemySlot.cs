using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlot : MonoBehaviour
{
    //몬스터 필드 위치 확인용

    // 몇번째 필드인지 정보 저장
    private int _index;

    public int Index => _index;

    [SerializeField]
    private GameObject _enemy = null;
    public GameObject Ehemy => _enemy;



    private void Start()
    {
        // 자신의 순서를 인덱스로 자동 할당 
        _index = transform.GetSiblingIndex();

        // SpawnManager에 자신을 등록
        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.RegisterEnemySlot(this);
        }
    }



}
