using UnityEngine;

public class EnemyHealth : BaseHealthScript
{
   [SerializeField] private Enemy _enemyLogic;
   [SerializeField] private EnemyAnimationsVisuals _enemyAnimationsVisuals;
   private bool trigered;
   public override void TakeDamage(float damage)
   {
      base.TakeDamage(damage);
      if (died && !trigered)
      {
         trigered = true;
         float deathTime = _enemyAnimationsVisuals.GetAnimationClipTime(EnemyAnimations.Death);
         _enemyLogic.DisableEnemyBehaviour();
         _enemyAnimationsVisuals.SwitchAnimation(EnemyAnimations.Death);
         Destroy(gameObject,deathTime);
      }
   }
}
