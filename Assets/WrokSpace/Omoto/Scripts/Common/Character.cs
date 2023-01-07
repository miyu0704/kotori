using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

using CharacterState;
using EventState;

public abstract class Character : MonoBehaviour
{
    /// <summary>
    /// �X�e�[�^�X�l
    /// </summary>
    protected class Parameter
    {
        public float dex;      // �ړ����x
    }

    /// <summary>
    /// �X�e�[�g�Ǘ�
    /// </summary>
    protected StateProcessor m_State;

    /// <summary>
    /// ���쏈��
    /// </summary>
    protected virtual void Ctrl() { }

    /// <summary>
    /// �X�e�[�g�`�F�b�N
    /// </summary>
    protected abstract void CheckState();

    /// <summary>
    /// �C�x���g�����i��_���[�W.�U�������Ȃǁj
    /// </summary>
    /// <param name="eventProcessor"></param>
    protected abstract void OnEvent(EventProcessor eventProcessor);
}
