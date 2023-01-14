using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility
{
    // 列挙体シリアライズ化構造
    //============================================
    public static class EnumUtil<T> where T : struct, IConvertible
    {
        private static Dictionary<string, T> m_Table = null;

        public static int count { get { return m_Table.Count; } }

        static EnumUtil()
        {
            // 型チェック
            var type = typeof(T);
            Assert.IsTrue(type.IsEnum, type.Name + " is not enum.");

            if (!type.IsEnum) return;

            // キャッシュ作成
            var values = Enum.GetValues(type);
            m_Table = new Dictionary<string, T>(values.Length);
            foreach (T value in values)
            {
                var name = value.ToString();
                m_Table.Add(name, value);
            }
        }

        /// <summary>
        /// 値取得
        /// </summary>
        /// <param name="name">キー</param>
        /// <returns>キーに対応した値</returns>
        public static T GetValue(string name)
        {
            return m_Table[name];
        }

        /// <summary>
        /// キー含有判定
        /// </summary>
        /// <param name="name">キー</param>
        /// <returns>キーが含まれるか</returns>
        public static bool IsDefined(string name)
        {
            return m_Table.ContainsKey(name);
        }

        // 以下 パルス処理
        //============================================
        public static T ParseOrDefault(string name)
        {
            T value;
            m_Table.TryGetValue(name, out value);
            return value;
        }

        public static T? ParseOrNull(string name)
        {
            T value;
            if (m_Table.TryGetValue(name, out value))
            {
                return value;
            }
            return null;
        }
    }

    [Serializable]
    public abstract class EnumStringBase
    {
        [SerializeField]
        private string m_Text = "";

        public virtual string text { get { return m_Text; } protected set { m_Text = value; } }
    }

    [Serializable]
    public class EnumString<T> : EnumStringBase where T : struct, IConvertible
    {
        private T? m_Cache = null;

        public T? cache { get { return m_Cache; } }

        public static Type type { get { return typeof(T); } }

        public T value
        {
            get
            {
                if (m_Cache == null)
                {
                    m_Cache = ParseOrDefault(text);
                }
                return (T)m_Cache;
            }
            set
            {
                text = value.ToString();
                m_Cache = value;
            }
        }

        public override string text
        {
            get { return base.text; }
            protected set
            {
                base.text = value;
                m_Cache = null;
            }
        }

        public EnumString() { }

        public EnumString(T value)
        {
            this.value = value;
        }

        public static bool IsDefined(string text)
        {
            return EnumUtil<T>.IsDefined(text);
        }

        public static T ParseOrDefault(string name)
        {
            return EnumUtil<T>.ParseOrDefault(name);
        }

        public static T? ParseOrNull(string name)
        {
            return EnumUtil<T>.ParseOrNull(name);
        }

        // 列挙変換
        public static implicit operator T(EnumString<T> obj) { return obj.value; }
    }
    //============================================

    // キー列挙体をシリアライズ化
    [Serializable]
    public class KeyCodeString : EnumString<KeyCode> { }

    //============================================
    // シングルトンアタッチ（特定のシーンのみで扱うオブジェクト向け）
    public class SingletonAttacher<T> where T : MonoBehaviour
    {
        static T m_Instance;
        public static T instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = UnityEngine.Object.FindObjectOfType<T>(true);
                    if (m_Instance == null)
                    {
                        Debug.LogError("GetError: Singleton<" + m_Instance.ToString() + ">");
                    }
                }
                return m_Instance;
            }
        }

        public static bool hasInstance => m_Instance != null;
    }
    //============================================
}