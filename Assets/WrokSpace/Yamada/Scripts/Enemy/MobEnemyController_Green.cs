using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MobEnemyController_Green : MobEnemyBase
    {
        private bool isChasing;


        public override void Init()
        {
            base.Init();

            isChasing = false;
        }


        protected override void Update()
        {
            Move();
        }


        private void Move()
        {
            if (isPlayerInRange && isChasing == false)
                isChasing = true;

            if (isChasing)
            {
                transform.position = Vector2.Lerp(transform.position, playerControllerRef.transform.position, 0.025f);
            }
        }

    }
}