using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Yellow : MobEnemyBase
    {
        // この敵が取る状態を列挙した変数
        private enum Status
        {
            Idle, Charging, Shooting
        }

        // 現在の状態を示す変数
        private Status currentStatus;

        // 弾を発射したか示すフラグ
        private bool isShot;


        // 固有の変数の初期化が入るため、overrideしている。
        public override void Init()
        {
            base.Init();

            isShot = false;
            currentStatus = Status.Idle;
        }


        // 射撃にチャージ時間を設けたいため、overrideしている。
        protected override void Update()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    if (isPlayerInRange)
                    {
                        StartCoroutine(ActionChangeWithTime());
                    }
                    break;

                case Status.Charging:
                    break;

                case Status.Shooting:
                    Shooting();
                    break;
            }
        }


        // レーザーを発射する。
        protected override void Shooting()
        {
            if (isShot)
                return;

            isShot = true;

            // 弾をManagerからもらう。
            EnemyBulletController_Yellow bullet = enemyBulletManagerRef.CreateBullet_Yellow();
            bullet.transform.position = this.transform.position;
            bullet.Init();

            // TODO. Rayを飛ばしてレーザーを作る。
            /*
            Vector2 origin = this.transform.position;
            Vector2 distance = Vector2.left * 1f;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.left);

            if (hit)
            {
                Debug.DrawRay(origin, Vector2.left, Color.yellow);
                Debug.Log(hit.distance);
            }
            */
        }


        /// <summary>
        /// 経過時間に応じて状態を変化させる。
        /// </summary>
        /// <returns></returns>
        private IEnumerator ActionChangeWithTime()
        {
            currentStatus = Status.Charging;

            yield return new WaitForSeconds(1);

            currentStatus = Status.Shooting;
        }


    }
}