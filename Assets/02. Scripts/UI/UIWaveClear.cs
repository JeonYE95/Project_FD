using TMPro;
using UnityEngine;

public class UIWaveClear : UIBase
{
    [SerializeField] private TMP_Text _gold;

    private void OnEnable()
    {
        _gold.text = WaveManager.Instance._currentWaveGold.ToString();
    }
}
