using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlot : MonoBehaviour
{
    //몬스터 필드 위치 확인용

    // 몇번째 필드인지 정보 저장
    public int Index { get; private set; }


    [SerializeField]
    private GameObject _enemy = null;
    public GameObject Ehemy => _enemy;





}
