using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : ActionHandler
{
    public override void ExecuteAction(BaseCharacter targetCharacter)
    {
        if (targetCharacter == null)
        {
            return;
        }

        //스킬 사용 추가
        //Instantiate 나 오브젝트 풀링으로 스킬 생성

        ResetCooldown();
    }
}
