using UnityEngine;

namespace Enemy
{
    public class EnemyBulletController_Purple : EnemyBulletBase
    {
        // 重力を使用するため、overrideしている。
        protected override void DoSettingsOfComponents()
        {
            base.DoSettingsOfComponents();
            myRigidbody.gravityScale = 1f;
        }
    }
}
