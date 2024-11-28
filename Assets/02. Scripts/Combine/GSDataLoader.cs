using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GSDataLoader : MonoBehaviour
{
    public string unitSheetURL =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vS5xVxEn-ZFBjQ3_4X8O4xyVENw2j2zHTrW3K_rkP0QP-F5GwCDzFFDpqdbLJjDOIZSTYK-5pUi_bNt/pub?gid=960464697&single=true&output=csv";
    public string combinationSheetURL =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vS5xVxEn-ZFBjQ3_4X8O4xyVENw2j2zHTrW3K_rkP0QP-F5GwCDzFFDpqdbLJjDOIZSTYK-5pUi_bNt/pub?gid=0&single=true&output=csv";

    public List<UnitData> LoadUnits = new List<UnitData>();
    public List<CombinationData> LoadCombinations = new List<CombinationData>();

    private void Start()
    {
        GetDataFromGS();
    }

    public void GetDataFromGS()
    {
        StartCoroutine(LoadUnitData());
        StartCoroutine(LoadCombinationData());
    }

    IEnumerator LoadUnitData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(unitSheetURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LoadUnits = UnitData.ParseList(request.downloadHandler.text);

                foreach (var unit in LoadUnits)
                {
                    Debug.Log($"ID: {unit.UnitID}, Name: {unit.Name}, Attack: {unit.Attack}, Defense: {unit.Defense}, Health: {unit.Health}");
                }
            }
        }
    }

    IEnumerator LoadCombinationData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(combinationSheetURL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                LoadCombinations = CombinationData.ParseList(request.downloadHandler.text);

                foreach (var combination in LoadCombinations)
                {
                    Debug.Log($"Combination ID: {combination.combinationID}, ResultUnitID: {combination.resultUnitID}, RequiredUnits: {string.Join(", ", combination.requiredUnits)}, IsHidden: {combination.isHidden}");
                }
            }
        }
    }
}