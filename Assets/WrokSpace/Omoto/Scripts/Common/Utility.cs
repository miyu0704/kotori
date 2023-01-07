using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility
{
    // �񋓑̃V���A���C�Y���\��
    //============================================
    public static class EnumUtil<T> where T : struct, IConvertible
    {
        private static Dictionary<string, T> m_Table = null;

        public static int count { get { return m_Table.Count; } }

        static EnumUtil()
        {
            // �^�`�F�b�N
            var type = typeof(T);
            Assert.IsTrue(type.IsEnum, type.Name + " is not enum.");

            if (!type.IsEnum) return;

            // �L���b�V���쐬
            var values = Enum.GetValues(type);
            m_Table = new Dictionary<string, T>(values.Length);
            foreach (T value in values)
            {
                var name = value.ToString();
                m_Table.Add(name, value);
            }
        }

        /// <summary>
        /// �l�擾
        /// </summary>
        /// <param name="name">�L�[</param>
        /// <returns>�L�[�ɑΉ������l</returns>
        public static T GetValue(string name)
        {
            return m_Table[name];
        }

        /// <summary>
        /// �L�[�ܗL����
        /// </summary>
        /// <param name="name">�L�[</param>
        /// <returns>�L�[���܂܂�邩</returns>
        public static bool IsDefined(string name)
        {
            return m_Table.ContainsKey(name);
        }

        // �ȉ� �p���X����
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

        // �񋓕ϊ�
        public static implicit operator T(EnumString<T> obj) { return obj.value; }
    }
    //============================================

    // �L�[�񋓑̂��V���A���C�Y��
    [Serializable]
    public class KeyCodeString : EnumString<KeyCode> { }

    //============================================
    // �V���O���g���A�^�b�`�i����̃V�[���݂̂ň����I�u�W�F�N�g�����j
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