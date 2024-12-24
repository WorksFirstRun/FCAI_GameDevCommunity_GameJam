using UnityEngine;
using System;

public class EnemyHealth : BaseHealthScript , IEnemyEnable
{
   [SerializeField] protected Enemy _enemyLogic;
   [SerializeField] protected BaseAnimationAndVisualsScript enemyAnimationsVisuals;
   [SerializeField] protected LootSO enemyLoot;
   private bool trigered;
   private const string SPAWNLOOTANDDISABLE = "SpawnLootAndDisable";
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
         Invoke(SPAWNLOOTANDDISABLE,deathTime);
      }
      else
      {
         onKnockBackTrigger?.Invoke();
      }
   }

   private void SpawnLootAndDisable()
   {
      DropSystem.Instance.DropItem(enemyLoot,transform.position);
      gameObject.SetActive(false); // for object pooling, i can't destroy it 
   }

   public void EnableBackTheEnemy()
   {
      gameObject.SetActive(true);
      this.SetCurrentHealth(maxHealth);
      _enemyLogic.EnableEnemyBehaviour();
      trigered = false;
      died = false;
   }

   private void OnDestroy()
   {
      onKnockBackTrigger = null;
   }
}
