using UnityEngine;

namespace Enemy
{
    public class EnemyBulletController_Red : EnemyBulletBase
    {
        // 移動速度を示す変数
        private float moveSpeed = 3f;


        /// <summary>
        /// 生成/再利用時に呼ばれる関数。プレイヤーの方向を向く処理のため、overrideしている。
        /// </summary>
        public override void Init()
        {
            base.Init();

            // プレイヤーの方向に向く。
            Vector3 diff = ((Vector3)playerPositionAtInit - this.transform.position).normalized;
            this.transform.rotation = Quaternion.FromToRotation(Vector3.up, diff);
        }


        /// <summary>
        /// 移動を行う。
        /// </summary>
        protected override void Move()
        {
            // 向いている方向に直線移動する。
            // TODO. モデルの回転を防ぐため、いずれやり方を変える。
            float currentSpeed = myRigidbody.velocity.magnitude;
            myRigidbody.AddRelativeForce(Vector2.up * (moveSpeed - moveSpeed * currentSpeed / moveSpeed));

        }

    }
}
