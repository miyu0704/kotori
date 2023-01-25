using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility.Attribute;

namespace Boss
{
    interface IBossProcessor
    {
        void OnDamage(float damage);
    }

    public abstract class BossProcessor : Character, IBossProcessor
    {
        // 値変数
        //============================================
        [System.Serializable]
        class MyParameter : Parameter
        {
            // TODO：ここにボスに関するパラメータを記入
            public float hp;
            public float atk;
        }

        [Header("パラメータ")]
        [SerializeField] MyParameter m_Param;

        [Header("動作分岐条件変数")]
        [SerializeField] float[] m_RandCondtion;    // 乱数条件（乱数による行動変化）
                                                    // ex. 0.60以下        移動（→攻撃）
                                                    // 　　0.60 - 0.80以下 長距離攻撃
                                                    // 　　0.80以上 　　　 特殊攻撃

        [SerializeField] float   m_DstCondition;    // 距離条件（距離による動作条件）
                                                    // ex. 設定値より高い -> ステップ動作

        [SerializeField] float[] m_HPCondition;     // 体力条件（残りHPによる動作条件）
                                                    // ex. 設定値を下回る -> 技が強化される（フェーズ移行, イベント）

        protected byte m_ActionID;  // 行動ID（ビット演算）
                                    // ex. 1 - 5 bit    乱数による行動分岐フラグ
                                    //     6     bit    距離による行動分岐フラグ
                                    //     7 - 8 bit    体力による行動分岐フラグ
                                    //
                                    // ex. 01000100：第二フェーズ, 乱数行動[2]
                                    //     00100001：第一フェーズ, 距離あり, 乱数行動[0]
                                    //     10101000：第三フェーズ, 距離あり, 乱数行動[3]
        public byte ActionID { get { return m_ActionID; } }

        // 衝突による処理
        //============================================
        protected void OnTriggerEnter2D(Collider2D collider)
        {
            // 被弾判定
            if (collider.gameObject.tag == "PlayerBullet")
            {
                var damage = collider.gameObject.GetComponent<BulletProcessor>().Damage;
                OnDamage(damage);

                Debug.Log("Hit");
            }
        }

        // 以下 オブジェクトメソッド
        //============================================
        /// <summary>
        /// Idle時から行動するとき、「行動乱数」「プレイヤーとの距離」「体力フェーズ」から行動IDをビット計算する
        /// ※基底クラスメソッドは実装例
        /// </summary>
        protected virtual void CalcActionID()
        {
            // IDのひな型
            int id = 0;

            // 行動乱数
            float r = Random.value;
            for(int i = 0; i < m_RandCondtion.Length; ++i)
            {
                if(r < m_RandCondtion[i])
                {
                    id += 0b1 << i;
                    break;
                }
            }

            // プレイヤーとの距離
            var stageObserver = SingletonAttacher<StageObserver>.instance;
            var dst = Vector2.Distance(stageObserver.PlayerVec3, stageObserver.BossVec3);
            if (m_DstCondition < dst) id += 0b100000;

            // 体力フェーズ
            for (int i = m_HPCondition.Length; 0 < i; --i)
            {
                if (m_Param.hp < m_HPCondition[i - 1])
                {
                    id += 0b1000000 << i;
                    break;
                }
            }

            m_ActionID = (byte)id;
            Debug.Log(m_ActionID);
        }

        protected abstract void Idle();

        protected abstract void Move();

        protected abstract void Jump();

        protected abstract void Floating();

        protected abstract void Attack();

        /// <summary>
        /// ダメージ処理
        /// </summary>
        /// <param name="damage">弾ダメージ</param>
        public void OnDamage(float damage)
        {
            m_Param.hp -= damage;
            Debug.Log(m_Param.hp);
            
            // その他ダメージを受けたときの処理
        }
    }
}