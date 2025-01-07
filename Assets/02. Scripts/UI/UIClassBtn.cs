using TMPro;
using UnityEngine;

public class UIClassBtn : UIBase
{
    [SerializeField] private TMP_Text _knightLevel;
    [SerializeField] private TMP_Text _archerLevel;
    [SerializeField] private TMP_Text _mageLevel;
    [SerializeField] private TMP_Text _healerLevel;
    [SerializeField] private TMP_Text _rogueLevel;
    [SerializeField] private TMP_Text _warriorLevel;

    private void OnEnable() 
    {
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Knight"].ToString();
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Archer"].ToString();
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Mage"].ToString();
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Healer"].ToString();
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Rogue"].ToString();
        _knightLevel.text = GameManager.Instance.playerData.ClassEnforce["Warrior"].ToString();
    }
}
