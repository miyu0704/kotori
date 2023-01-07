using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventState
{
    /// <summary>
    /// イベント実行管理クラス
    /// </summary>
    public abstract class EventProcessor
    {
        // デリゲート：処理を別オブジェクトに移譲すること
        public Action ExecAction { get; set; }

        // 実行管理
        public virtual void Execute()
        {
            if (ExecAction != null)
            {
                ExecAction();
            }
        }
    }
}
