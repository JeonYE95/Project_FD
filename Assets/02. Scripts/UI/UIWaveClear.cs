using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWaveClear : UIBase
{
    [SerializeField] private TMP_Text _gold;

    private void Start()
    {
        _gold.text = WaveManager.Instance._currentWaveGold.ToString();
    }
}
