using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Yellow : MobEnemyBase
    {
        private enum Status
        {
            Idle, Charging, Shooting
        }

        private Status currentStatus;

        private bool isShot;


        public override void Init()
        {
            base.Init();

            isShot = false;
            currentStatus = Status.Idle;
        }


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


        protected override void Shooting()
        {
            if (isShot)
                return;

            isShot = true;
            // 弾をManagerからもらう。
            EnemyBulletController_Yellow bullet = enemyBulletManagerRef.CreateBullet_Yellow();
            bullet.transform.position = this.transform.position;
            bullet.Init();
        }


        private IEnumerator ActionChangeWithTime()
        {
            currentStatus = Status.Charging;

            yield return new WaitForSeconds(1);

            currentStatus = Status.Shooting;
        }


    }
}