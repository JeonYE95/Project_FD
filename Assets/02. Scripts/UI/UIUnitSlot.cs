using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSlot : MonoBehaviour
{
    private RectTransform content;

    [SerializeField] private GameObject unitSlotPrefab;

    private List<Unit> inventoryUnits = new List<Unit>();

    void Start()
    {
        content = gameObject.GetComponent<RectTransform>();

        // 테스트 코드
        //Unit unit1 = gameObject.AddComponent<Unit>();
        //unit1.UnitName = "nametest1";
        //unit1.SpritePath = "Unit1";
        //inventoryUnits.Add(unit1);

        //Unit unit2 = gameObject.AddComponent<Unit>();
        //unit2.UnitName = "nametest2";
        //unit2.SpritePath = "Unit1";
        //inventoryUnits.Add(unit2);

        //Unit unit3 = gameObject.AddComponent<Unit>();
        //unit3.UnitName = "nametest3";
        //unit3.SpritePath = "Unit1";
        //inventoryUnits.Add(unit3);

        //Unit unit4 = gameObject.AddComponent<Unit>();
        //unit4.UnitName = "nametest4";
        //unit4.SpritePath = "Unit1";
        //inventoryUnits.Add(unit4);

        //Unit unit5 = gameObject.AddComponent<Unit>();
        //unit5.UnitName = "nametest5";
        //unit5.SpritePath = "Unit1";
        //inventoryUnits.Add(unit5);

        //Unit unit6 = gameObject.AddComponent<Unit>();
        //unit6.UnitName = "nametest6";
        //unit6.SpritePath = "Unit1";
        //inventoryUnits.Add(unit6);

        //Unit unit7 = gameObject.AddComponent<Unit>();
        //unit7.UnitName = "nametest7";
        //unit7.SpritePath = "Unit1";
        //inventoryUnits.Add(unit7);

        //Unit unit8 = gameObject.AddComponent<Unit>();
        //unit8.UnitName = "nametest8";
        //unit8.SpritePath = "Unit1";
        //inventoryUnits.Add(unit8);

        //Unit unit9 = gameObject.AddComponent<Unit>();
        //unit9.UnitName = "nametest9";
        //unit9.SpritePath = "Unit1";
        //inventoryUnits.Add(unit9);

        //Unit unit10 = gameObject.AddComponent<Unit>();
        //unit10.UnitName = "nametest10";
        //unit10.SpritePath = "Unit1";
        //inventoryUnits.Add(unit10);

        //Unit unit11 = gameObject.AddComponent<Unit>();
        //unit11.UnitName = "nametest11";
        //unit11.SpritePath = "Unit1";
        //inventoryUnits.Add(unit11);

        //Unit unit12 = gameObject.AddComponent<Unit>();
        //unit12.UnitName = "nametest12";
        //unit12.SpritePath = "Unit1";
        //inventoryUnits.Add(unit12);

        //Unit unit13 = gameObject.AddComponent<Unit>();
        //unit13.UnitName = "nametest13";
        //unit13.SpritePath = "Unit1";
        //inventoryUnits.Add(unit13);

        CreateUnitSlots();
    }

    private void CreateUnitSlots()
    {
       foreach (var unit in inventoryUnits)
       {
           GameObject go = Instantiate(unitSlotPrefab, content);

           TextMeshProUGUI unitNameTxt = go.GetComponentInChildren<TextMeshProUGUI>();
           Image unitImg = go.GetComponentInChildren<Image>();

           if (unitNameTxt !=  null)
           {
               unitNameTxt.text = unit.Name;
               Debug.Log(unitNameTxt);
           }
           else
               Debug.Log("TMP 컴포넌트 없음.");

           if (unitImg != null)
           {
            //    Sprite sprite = Resources.Load<Sprite>($"Sprite/{unit.SpritePath}");
            //    unitImg.sprite = sprite;
               Debug.Log(unitImg);
           }
           else
               Debug.Log("Image 컴포넌트 없음.");
       }
    }

    public void updateUnits(List<Unit> units)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
            // 새로운 유닛 목록으로 업데이트
            inventoryUnits = units;
            CreateUnitSlots();
    }

    public List<Unit> InventoryUnits
    {
        get => inventoryUnits;
        private set => inventoryUnits = value;
    }
}
