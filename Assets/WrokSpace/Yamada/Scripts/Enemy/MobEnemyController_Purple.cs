using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Purple : MobEnemyBase
    {
        // この敵が取る状態を列挙した変数
        private enum Status
        {
            Idle, MovingToOverhead, ChasingPlayer, ShootingBullet, ReturnToBase
        }

        // 現在の状態を示す変数
        private Status currentStatus;

        // 弾を発射したか示すフラグ
        private bool isShot = false;


        // 固有の変数の初期化が入るため、overrideしている。
        public override void Init()
        {
            base.Init();

            currentStatus = Status.Idle;
            isShot = false;
        }


        // FixedUpdateを主に使用するため、Updateを無効化する。
        protected override void Update() { }


        /// <summary>
        /// FixedUpdate
        /// </summary>
        private void FixedUpdate()
        {
            switch (currentStatus)
            {
                case Status.Idle:
                    if (isPlayerInRange)
                    {
                        StartCoroutine(ActionChangeWithTime());
                    }
                    break;

                case Status.MovingToOverhead:
                    MoveToOverhead();
                    break;

                case Status.ChasingPlayer:
                    ChasingPlayer();
                    break;

                case Status.ShootingBullet:
                    Shooting();
                    break;

                case Status.ReturnToBase:
                    ReturnToBase();
                    break;

            }
        }


        /// <summary>
        /// プレイヤーの頭上に向かって移動する。
        /// </summary>
        private void MoveToOverhead()
        {
            Vector2 offsetYFromPlayer = new Vector2(0, 3f);
            transform.position = Vector2.Lerp(transform.position, (Vector2)playerControllerRef.transform.position + offsetYFromPlayer, 0.1f);
        }


        /// <summary>
        /// プレイヤーとX軸を合わせるように移動する。
        /// </summary>
        private void ChasingPlayer()
        {
            Vector2 offsetYFromPlayer = new Vector2(0, 3f);
            transform.position = Vector2.Lerp(transform.position, (Vector2)playerControllerRef.transform.position + offsetYFromPlayer, 0.05f);
        }


        /// <summary>
        /// 弾を発射する。
        /// </summary>
        protected override void Shooting()
        {
            if (isShot)
                return;

            isShot = true;

            // 弾をManagerからもらう。
            EnemyBulletController_Purple bullet = enemyBulletManagerRef.CreateBullet_Purple();
            bullet.transform.position = this.transform.position;
            bullet.Init();
        }


        /// <summary>
        /// 画面外に消えるように移動する。
        /// </summary>
        private void ReturnToBase()
        {
            const float moveSpeed = 3f;
            float currentSpeed = myRigidbody.velocity.magnitude;
            myRigidbody.AddForce(Vector2.left * (moveSpeed - moveSpeed * currentSpeed / moveSpeed));
        }


        /// <summary>
        /// 経過時間に応じて状態を変化させる。
        /// </summary>
        /// <returns></returns>
        private IEnumerator ActionChangeWithTime()
        {
            currentStatus = Status.MovingToOverhead;

            yield return new WaitForSeconds(1);

            currentStatus = Status.ChasingPlayer;

            yield return new WaitForSeconds(2);

            currentStatus = Status.ShootingBullet;

            yield return new WaitForSeconds(1);

            currentStatus = Status.ReturnToBase;
        }

    }
}