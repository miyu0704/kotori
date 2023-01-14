using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

/// <summary>
/// キャラ状態
/// </summary>
namespace CharacterState
{
    /// <summary>
    /// ステート優先度列挙体
    /// </summary>
    public enum StatePriority
    {
        e_IDLE = 0,
        e_MOVE,
        e_JUMP,
        e_Sliding = e_JUMP,
        e_ATTACK,
        e_DAMAGE
    }

    /// <summary>
    /// ステート実行管理クラス
    /// </summary>
    public abstract class StateProcessor
    {
        /// <summary>
        /// 参照キャラクター
        /// </summary>
        Character character;

        // デリゲート：処理を別オブジェクトに移譲する
        public Action ExecAction { get; set; }

        // 実行管理
        public virtual void Execute() 
        {
            if(ExecAction != null)
            {
                ExecAction();
            }
        }

        // ステート優先度取得
        public abstract StatePriority GetPriority();

        // ステート名取得
        public abstract string GetStateName();

        // ステート優先度比較 operator
        //============================================
        public static bool operator <(StateProcessor processor, StatePriority priority)
        {
            return processor.GetPriority() < priority;
        }
        public static bool operator >(StateProcessor processor, StatePriority priority)
        {
            return processor.GetPriority() > priority;
        }
    }

    // 以下 キャラクター共通ステート
    //============================================
    public class IdleState : StateProcessor
    {
        public override StatePriority GetPriority()
        {
            return StatePriority.e_IDLE;
        }

        public override string GetStateName()
        {
            return "state:Idle";
        }
    }

    public class MoveState : StateProcessor
    {
        public override StatePriority GetPriority()
        {
            return StatePriority.e_MOVE;
        }

        public override string GetStateName()
        {
            return "state:Move";
        }
    }

    public class JumpState : StateProcessor
    {
        public JumpState(ref Rigidbody2D rb2d, Vector2 velocity)
        {
            rb2d.velocity = velocity;
        }

        public override StatePriority GetPriority()
        {
            return StatePriority.e_JUMP;
        }

        public override string GetStateName()
        {
            return "state:Jump";
        }
    }

    public class AttackState : StateProcessor
    {
        public override StatePriority GetPriority()
        {
            return StatePriority.e_ATTACK;
        }

        public override string GetStateName()
        {
            return "state:Attack";
        }
    }

    public class DamageState : StateProcessor
    {
        public override StatePriority GetPriority()
        {
            return StatePriority.e_DAMAGE;
        }

        public override string GetStateName()
        {
            return "state:Damage";
        }
    }
};
