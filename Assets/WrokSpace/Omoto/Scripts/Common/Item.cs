using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

/// <summary>
/// Item�F�L�����N�^�[�ɕt������l�X�ȃI�u�W�F�N�g���_��ɑΉ��ł���悤�ɍ쐬 <br />
/// �@�@�@�i�n�[�h�R�[�f�B���O�΍�j
/// </summary>
namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        // �A�C�e�����X�g
        static List<ItemProcessor> m_ItemLists = new List<ItemProcessor>();

        /// <summary>
        /// �A�C�e���o�^
        /// </summary>
        /// <param name="item">�o�^����A�C�e��</param>
        public ItemProcessor AddItem(ItemProcessor item)
        {
            m_ItemLists.Add(item);
            return item;
        }

        /// <summary>
        /// �A�C�e���o�^����
        /// </summary>
        public void ClearItems() => m_ItemLists.Clear();

        /// <summary>
        /// �A�C�e���擾�i�����j
        /// </summary>
        /// <param name="id">�A�C�e���o�^��ID</param>
        public ItemProcessor GetItem(int id)
        {
            return m_ItemLists[id].Clone();
        }
    }

    /// <summary>
    /// �A�C�e�����s�Ǘ��N���X
    /// </summary>
    public abstract class ItemProcessor : MonoBehaviour
    {
        // �f���Q�[�g�F������ʃI�u�W�F�N�g�Ɉڏ����邱��
        public Action ExecAction { get; set; }

        public ItemProcessor Clone()
        {
            return MemberwiseClone() as ItemProcessor;
        }

        public virtual void Execute()
        {
            if(ExecAction != null)
            {
                ExecAction();
            }
        }
    }

    /// <summary>
    /// �f�R���[�g�A�C�e�����s�Ǘ��N���X <br/>
    /// �i���̃A�C�e�����x�[�X�ɂ���Ɍʂ̏����𓱓��������Ƃ��Ɉ����j
    /// </summary>
    public class ItemDecorator : ItemProcessor
    {
        // �f�R���[�g��A�C�e��
        ItemProcessor m_Item;

        // �f���Q�[�g�F������ʃI�u�W�F�N�g�Ɉڏ����邱��
        public Action DecoAction { get; set; }

        public ItemDecorator(ItemProcessor item)
        {
            m_Item = item;
        }

        public override void Execute()
        {
            // �f�R���[�g����
            if(DecoAction != null)
            {
                DecoAction();
            }

            // �A�C�e������
            m_Item.Execute();
        }
    }
}
