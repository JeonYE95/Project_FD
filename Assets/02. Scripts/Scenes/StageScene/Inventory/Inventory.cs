using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    enum Buttons
    {

        normalButton,
        rareButton,
        uniqueButton,

    }

    private List<Unit> normalUnit = new List<Unit>();
    private List<Unit> rareUnit = new List<Unit>();
    private List<Unit> uniqueUnit = new List<Unit>();


    //private UIUnitSlot UnitList = GetComponentInChildren<UIUnitSlot>();


    public void Start()
    {
        Button normalButton = GetComponentsInChildren<Button>()[(int)Buttons.normalButton];
        Button rareButton = GetComponentsInChildren<Button>()[(int)Buttons.rareButton];
        Button uniqueButton = GetComponentsInChildren<Button>()[(int)Buttons.uniqueButton];


        normalButton.onClick.AddListener(() => UpdateSlots(normalUnit));
        rareButton.onClick.AddListener(() => UpdateSlots(rareUnit));
        uniqueButton.onClick.AddListener(() => UpdateSlots(uniqueUnit));
    }



    public void SwapItem()
    {
        // 인벤토리에서 꺼낸 캐릭터와 필드 위의 캐릭터를 서로 바꾸어야 함.


    }

  

    private void UpdateSlots(List<Unit> units)
    {
       
   
        // 새로운 유닛 데이터로 슬롯 업데이트
        /*
         
        for (int i = 0; i < Mathf.Min(units.Count, UnitList.inventoryUnits.Count); i++)
        {
            // 프로퍼티 만들어 달라 요청
            UnitList.inventoryUnits[i] = units[i];
        }
         
         */
    }

}
