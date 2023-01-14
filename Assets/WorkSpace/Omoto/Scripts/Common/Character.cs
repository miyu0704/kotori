using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

using CharacterState;
using EventState;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// ステータス値
    /// </summary>
    protected class Parameter
    {
        public float dex;      // 移動速度
    }

    /// <summary>
    /// ステート管理
    /// </summary>
    protected StateProcessor m_State;

    /// <summary>
    /// 操作処理
    /// </summary>
    protected virtual void Ctrl() { }

    /// <summary>
    /// ステートチェック
    /// </summary>
    protected abstract void CheckState();

    /// <summary>
    /// イベント処理（被ダメージ.攻撃時ンなど）
    /// </summary>
    /// <param name="eventProcessor"></param>
    protected abstract void OnEvent(EventProcessor eventProcessor);
}
