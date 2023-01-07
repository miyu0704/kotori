using UnityEngine;

namespace Enemy
{
    public class EnemyBulletController_Blue : EnemyBulletBase
    {
        // 弾の射出角度
        private const float shootingAngle = 60f;


        // 放物線を描く射出処理を挟むため、overrideしている。
        public override void Init()
        {
            base.Init();

            AddForceOfShooting_OneTime();
        }


        // 放物線に重力を使用するため、overrideしている。
        protected override void DoSettingsOfComponents()
        {
            base.DoSettingsOfComponents();

            myRigidbody.gravityScale = 1f;
        }


        /// <summary>
        /// 弾を放物線状に射出する処理を行う。
        /// </summary>
        private void AddForceOfShooting_OneTime()
        {
            Vector2 velocity = CalculateVelocity(transform.position, playerPositionAtInit, shootingAngle);
            myRigidbody.AddForce(velocity * myRigidbody.mass, ForceMode2D.Impulse);
        }


        /// <summary>
        /// ref: https://qiita.com/_udonba/items/a71e11c8dd039171f86c から丸パクリ
        /// 標的に命中する射出速度の計算
        /// </summary>
        /// <param name="startPoint">射出開始座標</param>
        /// <param name="targetPoint">標的の座標</param>
        /// <returns>射出速度</returns>
        private Vector3 CalculateVelocity(Vector3 startPoint, Vector3 targetPoint, float angle)
        {
            // 射出角をラジアンに変換
            float rad = angle * Mathf.Deg2Rad;

            // 水平方向の距離x
            float x = Vector2.Distance(new Vector2(startPoint.x, startPoint.z), new Vector2(targetPoint.x, targetPoint.z));

            // 垂直方向の距離y
            float y = startPoint.y - targetPoint.y;

            // 斜方投射の公式を初速度について解く
            float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

            if (float.IsNaN(speed))
            {
                // 条件を満たす初速を算出できなければVector3.zeroを返す
                return Vector3.zero;
            }

            return (new Vector3(targetPoint.x - startPoint.x, x * Mathf.Tan(rad), targetPoint.z - startPoint.z).normalized * speed);
        }
    }
}
