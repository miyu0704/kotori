using UnityEngine;

namespace Enemy
{
    public class EnemyBulletController_Purple : EnemyBulletBase
    {
        protected override void DoSettingsOfComponents()
        {
            base.DoSettingsOfComponents();
            myRigidbody.gravityScale = 1f;
        }

    }
}
