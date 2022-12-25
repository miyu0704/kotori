using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Purple : MobEnemyBase
    {
        private enum Status
        {
            Idle, MovingToOverhead, ChasingPlayer, ShootingBullet, ReturnToBase
        }

        private Status currentStatus;

        private bool isShot = false;


        public override void Init()
        {
            base.Init();

            currentStatus = Status.Idle;
            isShot = false;
        }


        // FixedUpdateを主に使用するため、Updateを無効化する。
        protected override void Update() { }


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


        private void MoveToOverhead()
        {
            Vector2 offsetYFromPlayer = new Vector2(0, 3f);
            transform.position = Vector2.Lerp(transform.position, (Vector2)playerControllerRef.transform.position + offsetYFromPlayer, 0.1f);
        }

        private void ChasingPlayer()
        {
            Vector2 offsetYFromPlayer = new Vector2(0, 3f);
            transform.position = Vector2.Lerp(transform.position, (Vector2)playerControllerRef.transform.position + offsetYFromPlayer, 0.05f);
        }


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


        private void ReturnToBase()
        {
            const float moveSpeed = 3f;
            float currentSpeed = myRigidbody.velocity.magnitude;
            myRigidbody.AddForce(Vector2.left * (moveSpeed - moveSpeed * currentSpeed / moveSpeed));
        }


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