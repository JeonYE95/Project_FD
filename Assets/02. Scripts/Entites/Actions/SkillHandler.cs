using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : ActionHandler
{
    public SkillData skillData;
    float skillDuration;


    /*public override void ExecuteAction(BaseUnit targetCharacter)
    {
        if (targetCharacter == null)
        {
            return;
        }

        Debug.Log($"{gameObject.name} 스킬 사용됌");
        //스킬 사용 추가
        //Instantiate 나 오브젝트 풀링으로 스킬 생성

        ResetCooldown();
    }*/
}
