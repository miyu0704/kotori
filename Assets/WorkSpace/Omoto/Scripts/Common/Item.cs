using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

/// <summary>
/// Item：キャラクターに付随する様々なオブジェクトを柔軟に対応できるように作成 <br />
/// 　　　（ハードコーディング対策）
/// </summary>
namespace Item
{
    public class ItemManager : MonoBehaviour
    {
        // アイテムリスト
        static List<ItemProcessor> m_ItemLists = new List<ItemProcessor>();

        /// <summary>
        /// アイテム登録
        /// </summary>
        /// <param name="item">登録するアイテム</param>
        public ItemProcessor AddItem(ItemProcessor item)
        {
            m_ItemLists.Add(item);
            return item;
        }

        /// <summary>
        /// アイテム登録解除
        /// </summary>
        public void ClearItems() => m_ItemLists.Clear();

        /// <summary>
        /// アイテム取得（生成）
        /// </summary>
        /// <param name="id">アイテム登録順ID</param>
        public ItemProcessor GetItem(int id)
        {
            return m_ItemLists[id].Clone();
        }
    }

    /// <summary>
    /// アイテム実行管理クラス
    /// </summary>
    public abstract class ItemProcessor : MonoBehaviour
    {
        // デリゲート：処理を別オブジェクトに移譲すること
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
    /// デコレートアイテム実行管理クラス <br/>
    /// （元のアイテムをベースにさらに個別の処理を導入したいときに扱う）
    /// </summary>
    public class ItemDecorator : ItemProcessor
    {
        // デコレート先アイテム
        ItemProcessor m_Item;

        // デリゲート：処理を別オブジェクトに移譲すること
        public Action DecoAction { get; set; }

        public ItemDecorator(ItemProcessor item)
        {
            m_Item = item;
        }

        public override void Execute()
        {
            // デコレート処理
            if(DecoAction != null)
            {
                DecoAction();
            }

            // アイテム処理
            m_Item.Execute();
        }
    }
}
