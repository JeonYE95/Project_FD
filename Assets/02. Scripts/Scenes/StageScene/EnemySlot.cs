using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySlot : MonoBehaviour
{
    //몬스터 필드 위치 확인용

    // 몇번째 필드인지 정보 저장
    [SerializeField]
    private int _index;

    public int Index => _index;

    [SerializeField]
    private GameObject _enemy = null;
    public GameObject Enemy => _enemy;



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



    public void SetEnemy(GameObject enemy)
    {
        _enemy = enemy;

        if (enemy != null)
        {
            // UI 좌표를 월드 좌표로 변환
            Vector3 uiPosition = transform.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(
                uiPosition.x,
                uiPosition.y,
                Camera.main.nearClipPlane));
            worldPosition.z = 0; // 2D 게임이므로 z축을 0으로 설정

            // 부모-자식 관계 설정
            enemy.transform.SetParent(transform);
            // 위치 설정
            enemy.transform.position = worldPosition;
        }
    }


}
