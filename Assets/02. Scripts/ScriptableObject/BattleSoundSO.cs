using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleSoundSO", menuName = "Config/BattleSoundSO")]
public class BattleSoundSO : ScriptableObject
{
    public ActionSound defaultSound;

    [System.Serializable]
    public class ActionSound
    {
        public int unitID;             // 유닛 별 소리
        public string soundAssetName;
        public string skillSoundAssetName;
        public float volume = 1.0f;
    }

    public List<ActionSound> actionSounDatas;
}
