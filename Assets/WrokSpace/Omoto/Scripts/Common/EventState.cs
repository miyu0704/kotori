using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventState
{
    /// <summary>
    /// �C�x���g���s�Ǘ��N���X
    /// </summary>
    public abstract class EventProcessor
    {
        // �f���Q�[�g�F������ʃI�u�W�F�N�g�Ɉڏ����邱��
        public Action ExecAction { get; set; }

        // ���s�Ǘ�
        public virtual void Execute()
        {
            if (ExecAction != null)
            {
                ExecAction();
            }
        }
    }
}
