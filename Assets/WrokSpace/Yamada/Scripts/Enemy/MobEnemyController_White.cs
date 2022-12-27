using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_White : MobEnemyBase
    {
        private bool isCreatedGreen = false;

        public override void Init()
        {
            base.Init();

            isCreatedGreen = false;
        }


        protected override void Shooting()
        {
            if (isCreatedGreen)
                return;

            if (isPlayerInRange)
            {
                isCreatedGreen = true;

                // 召喚するをManagerからもらう。
                MobEnemyController_Green createdGreen = mobEnemyManagerRef.CreateEnemy_Green();
                createdGreen.transform.position = (Vector2)playerControllerRef.transform.position + new Vector2(-1, 0);
                createdGreen.Init();
            }
        }

    }
}