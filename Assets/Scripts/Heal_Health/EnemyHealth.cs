using UnityEngine;
using System;

public class EnemyHealth : BaseHealthScript
{
   [SerializeField] protected Enemy _enemyLogic;
   [SerializeField] protected BaseAnimationAndVisualsScript enemyAnimationsVisuals;
   [SerializeField] protected LootSO enemyLoot;
   private bool trigered;
   private const string SPAWNLOOTANDDESTROY = "SpawnLootAndDestroy";
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
         Invoke(SPAWNLOOTANDDESTROY,deathTime);
      }
      else
      {
         onKnockBackTrigger?.Invoke();
      }
   }

   private void SpawnLootAndDestroy()
   {
      DropSystem.Instance.DropItem(enemyLoot,transform.position);
      Destroy(gameObject);
   }
   
   private void OnDestroy()
   {
      onKnockBackTrigger = null;
   }
}
