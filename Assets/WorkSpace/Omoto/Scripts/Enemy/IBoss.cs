using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boss
{
    interface IBossProcessor
    {
        void OnDamage();
    }

    public abstract class BossProcessor : Character, IBossProcessor
    {
        // 衝突による処理
        //============================================
        private void OnCollisionEnter2D(Collision2D collision)
        {

        }

        public void OnDamage()
        {

        }
    }
}