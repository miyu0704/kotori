using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Red : MobEnemyBase
    {
        // 弾を発射してから、次の弾が発射できるまでにかかる時間
        private const float reloadTime_sec = 1f;


        /// <summary>
        /// 弾を発射する。
        /// </summary>
        protected override void Shooting()
        {
            if (isBulletReloading.flag)
                return;

            if (isPlayerInRange == false)
                return;

            // 指定秒数後に再び弾が撃てるようにする。
            isBulletReloading.flag = true;
            StartCoroutine(ToggleFlagAfterSeconds(isBulletReloading, reloadTime_sec));

            // 弾をManagerからもらう。
            EnemyBulletController_Red bullet = enemyBulletManagerRef.CreateBullet_Red();
            bullet.transform.position = this.transform.position;
            bullet.Init();
        }
    }
}