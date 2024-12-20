using UnityEngine;
using System;

public class EnemyHealth : BaseHealthScript
{
   [SerializeField] private Enemy _enemyLogic;
   [SerializeField] private BaseAnimationAndVisualsScript enemyAnimationsVisuals;
   private bool trigered;
   public event Action onKnockBackTrigger;
   
   public override void TakeDamage(float damage)
   {
      base.TakeDamage(damage);
      if (died && !trigered)
      {
         trigered = true;
         float deathTime = enemyAnimationsVisuals.GetAnimationClipTime(enemyAnimationsVisuals.GetDeathAnimationEnum());
         _enemyLogic.DisableEnemyBehaviour();
         enemyAnimationsVisuals.SwitchAnimation(enemyAnimationsVisuals.GetDeathAnimationEnum());
         Destroy(gameObject,deathTime);
      }
      else
      {
         onKnockBackTrigger?.Invoke();
      }
   }
}
