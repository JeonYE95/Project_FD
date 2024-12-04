using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitSlotTest : MonoBehaviour
{
   
    private RectTransform content;

    [SerializeField] private GameObject unitSlotPrefab;

    [SerializeField]
    private List<Unit> inventoryUnits = new List<Unit>();

    public List<Unit> InventoryUnits
    {
        get => inventoryUnits;
        private set => inventoryUnits = value;
    }
    void Start()
    {
        content = gameObject.GetComponent<RectTransform>();

        // 테스트 코드
        Unit unit1 = gameObject.AddComponent<Unit>();
        unit1.Name = "Archer";
        unit1.Grade = "Common";
        inventoryUnits.Add(unit1);

        Unit unit2 = gameObject.AddComponent<Unit>();
        unit2.Name = "Healer";
        unit2.Grade = "Common";
        inventoryUnits.Add(unit2);

     
        CreateUnitSlots();
    }



    private void CreateUnitSlots()
    {
        foreach (var unit in inventoryUnits)
        {
            GameObject go = Instantiate(unitSlotPrefab, content);

            TextMeshProUGUI unitNameTxt = go.GetComponentInChildren<TextMeshProUGUI>();
            Image unitImg = go.GetComponentInChildren<Image>();

            if (unitNameTxt != null)
            {
                unitNameTxt.text = unit.Name;
                Debug.Log(unitNameTxt);
            }
            else
                Debug.Log("TMP 컴포넌트 없음.");

            if (unitImg != null)
            {
                Sprite sprite = Resources.Load<Sprite>($"Sprite/{unit.Name}");
                unitImg.sprite = sprite;
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

}
