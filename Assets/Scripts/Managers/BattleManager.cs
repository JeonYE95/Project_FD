using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    private List<BaseCharacter> allCharacters = new List<BaseCharacter>();
    public List<BaseCharacter> players = new List<BaseCharacter>();
    public List<BaseCharacter> enemies = new List<BaseCharacter>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BaseCharacter GetClosetTarget(BaseCharacter unit)
    {
        List<BaseCharacter> targetList = unit.isPlayerCharacter ? enemies : players;
        BaseCharacter targer;

        return targer = null;
    }
}
