using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCharacter : MonoBehaviour
{
    //캐릭터 정보 변수로 가짐 
    private int _count;
    private int MaxQuantity = 99;

    public int RemainCapacity => MaxQuantity - _count;

    public int Count => _count;


    public void CharacterAddCount(int count)
    {

        _count = count < MaxQuantity ? _count += count : MaxQuantity;

    }


    // 조합, 필드에 올려 놓을 시 캐릭터 숫자 감소 
    public void CharacerSubtractCount(int count)
    {
        _count = Count - count < 0 ? _count -= count : 0;

    }



}
