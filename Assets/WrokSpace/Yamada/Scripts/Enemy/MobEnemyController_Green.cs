using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Green : MobEnemyBase
    {
        // プレイヤーを追いかけているか示すフラグ
        private bool isChasingPlayer;


        // 固有の変数の初期化が入るため、overrideしている。
        public override void Init()
        {
            base.Init();

            isChasingPlayer = false;
        }


        // 弾を撃たないため、便宜上overrideしている。
        protected override void Update()
        {
            Move();
        }


        /// <summary>
        /// プレイヤーに向かって移動を行う。
        /// </summary>
        private void Move()
        {
            if (isPlayerInRange && isChasingPlayer == false)
                isChasingPlayer = true;

            if (isChasingPlayer)
            {
                // transform.position = Vector2.Lerp(transform.position, playerControllerRef.transform.position, 0.025f);
                transform.position = Vector2.MoveTowards(transform.position, playerControllerRef.transform.position, Time.deltaTime * 2);
            }
        }

    }
}