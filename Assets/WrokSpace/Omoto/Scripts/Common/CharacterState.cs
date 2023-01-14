using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

/// <summary>
/// �L�������
/// </summary>
namespace CharacterState
{
    /// <summary>
    /// �X�e�[�g�D��x�񋓑�
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
    /// �X�e�[�g���s�Ǘ��N���X
    /// </summary>
    public abstract class StateProcessor
    {
        /// <summary>
        /// �Q�ƃL�����N�^�[
        /// </summary>
        Character character;

        // �f���Q�[�g�F������ʃI�u�W�F�N�g�Ɉڏ�����
        public Action ExecAction { get; set; }

        // ���s�Ǘ�
        public virtual void Execute() 
        {
            if(ExecAction != null)
            {
                ExecAction();
            }
        }

        // �X�e�[�g�D��x�擾
        public abstract StatePriority GetPriority();

        // �X�e�[�g���擾
        public abstract string GetStateName();

        // �X�e�[�g�D��x��r operator
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

    // �ȉ� �L�����N�^�[���ʃX�e�[�g
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
