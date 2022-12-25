using UnityEngine;

namespace Enemy
{
    public class EnemyBulletController_Yellow : EnemyBulletBase
    {
        private const float moveSpeed = 30f;


        protected override void Move()
        {
            float currentSpeed = myRigidbody.velocity.magnitude;
            myRigidbody.AddForce(Vector2.left * (moveSpeed - moveSpeed * currentSpeed / moveSpeed));
        }

    }
}
