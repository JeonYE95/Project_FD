using System;
using UnityEngine;

public class BuffInfo
{
    public Coroutine Timer { get; set; } // 버프 타이머 코루틴
    public Action ResetAction { get; set; } // 리셋 액션

    public BuffInfo(Coroutine timer, Action resetAction)
    {
        Timer = timer;
        ResetAction = resetAction;
    }
}