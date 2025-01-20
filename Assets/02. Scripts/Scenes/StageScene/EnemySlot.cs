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

        StartCoroutine(DelayedRegister());


    }

    private IEnumerator DelayedRegister()
    {
        // 한 프레임 기다려서 UI 레이아웃이 업데이트되도록 함
        yield return null;

        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.RegisterEnemySlot(this);
        }
    }

    public void SetIndex(int index)
    {
        _index = index;
    }



    public void SetEnemy(GameObject enemy)
    {
        _enemy = enemy;

        if (enemy != null)
        {
            // SpawnManager의 Enemies 오브젝트 아래에 배치
            enemy.transform.SetParent(SpawnManager.Instance.EnemiesParent);

            // SpawnManager에서 저장된 위치 사용
            Vector3 worldPos = SpawnManager.Instance.GetEnemyPosition(_index);
            enemy.transform.position = worldPos;




        }
    }


}
