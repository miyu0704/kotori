using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_White : MobEnemyBase
    {
        // この敵が取る状態を列挙した変数
        private enum Status
        {
            Idle, Charging, Summoning
        }

        // 現在の状態を示す変数
        private Status currentStatus;

        // 召喚を行ったか示すフラグ
        private bool isSummoned = false;


        // 固有の変数の初期化が入るため、overrideしている。
        public override void Init()
        {
            base.Init();

            currentStatus = Status.Idle;
            isSummoned = false;
        }


        // 射撃を行わないため、overrideしている。
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

                case Status.Summoning:
                    Summoning();
                    break;
            }
        }


        /// <summary>
        /// ザコ妖精緑を召喚する。
        /// </summary>
        private void Summoning()
        {
            if (isSummoned)
                return;

            if (isPlayerInRange)
            {
                isSummoned = true;

                // 召喚するをManagerからもらう。
                MobEnemyController_Green createdGreen = mobEnemyManagerRef.CreateEnemy_Green();
                createdGreen.transform.position = (Vector2)playerControllerRef.transform.position + new Vector2(-4, 0);
                createdGreen.Init();

                Disable();
            }
        }


        /// <summary>
        /// 経過時間に応じて状態を変化させる。
        /// </summary>
        /// <returns></returns>
        private IEnumerator ActionChangeWithTime()
        {
            currentStatus = Status.Charging;

            yield return new WaitForSeconds(2);

            currentStatus = Status.Summoning;
        }

    }
}