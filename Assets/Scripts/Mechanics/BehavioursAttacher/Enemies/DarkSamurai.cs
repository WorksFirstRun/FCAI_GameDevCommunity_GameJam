using System;
using BehaviourTreeNamespace;
using BehaviourTreeNamespace.EnemyAi_ActionNodes;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;


public class DarkSamurai : Enemy
{
   private void Start()
   {
      Context enemyLeafsContext = new Context
      {
         rigidbody = rb,
         moveSpeed = movementSpeed,
         entityTransform = enemyTransform,
         firePoint = attackPoint,
         animation_VisualsHandler = enemyAnimationVisuals
      };
      enemyLeafsContext.SetEnemyAreas(chasingArea, attackArea, castSpellArea, detectionLayerMask);


      _behaviourTree = new BehaviourTree();
      Selector selector = new Selector(_behaviourTree);
      
      _behaviourTree.AddChild(selector);
      
      FallBackNode idle_RoamFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Idle_Roaming, selector);
      FallBackNode attackFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Attack, selector);
      FallBackNode chaseFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Chase, selector);
      FallBackNode knockBackFallBack =
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.KnockBack, selector);
      Sequence idle_roam_sequencer = new Sequence(idle_RoamFallBack);
      
      Enemy_IdleActionNode<DarkSamuraiAnimation> enemyIdleActionNode = 
         new Enemy_IdleActionNode<DarkSamuraiAnimation>(enemyLeafsContext, idle_roam_sequencer, idleTime,obstacleLayerMask
      ,DarkSamuraiAnimation.Idle);
      
      Enemy_RoamActionNode<DarkSamuraiAnimation> enemyRoamActionNode = 
         new Enemy_RoamActionNode<DarkSamuraiAnimation>(enemyLeafsContext, idle_roam_sequencer,DarkSamuraiAnimation.Run);
      Action[] attacks = new Action[]
      {
         GiveDamage
      };
      DarkSamuraiAnimation[] attacksName = new DarkSamuraiAnimation[] { DarkSamuraiAnimation.Attack };
      
      Enemy_AttackActionNode<DarkSamuraiAnimation> enemyAttackActionNode = new Enemy_AttackActionNode<DarkSamuraiAnimation>(enemyLeafsContext, attackFallBack, 
         0.6f, attacks,attacksName,DarkSamuraiAnimation.Idle);
      Enemy_ChaseActionNode<DarkSamuraiAnimation> enemyChaseActionNode = 
         new Enemy_ChaseActionNode<DarkSamuraiAnimation>(enemyLeafsContext, chaseFallBack,DarkSamuraiAnimation.Run);
      Enemy_KnockBackActionNode<DarkSamuraiAnimation> enemyKnockBackActionNode = new Enemy_KnockBackActionNode<DarkSamuraiAnimation>(enemyLeafsContext, knockBackFallBack,
         _enemyHealth, knockBackStrength,DarkSamuraiAnimation.Hit);
      
      attackFallBack.AddChild(enemyAttackActionNode); // fall backs
      idle_RoamFallBack.AddChild(idle_roam_sequencer); // fall backs
      chaseFallBack.AddChild(enemyChaseActionNode);
      knockBackFallBack.AddChild(enemyKnockBackActionNode);
      
      idle_roam_sequencer.AddChild(enemyIdleActionNode); // extra 
      idle_roam_sequencer.AddChild(enemyRoamActionNode);
      
      
      selector.AddChild(idle_RoamFallBack); // attach fall backs to selector
      selector.AddChild(attackFallBack);
      selector.AddChild(chaseFallBack);
      selector.AddChild(knockBackFallBack);
   }

   private void Update()
   {
      _behaviourTree.Evaluate();
   }
   
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      // Draw a wireframe circle/sphere in 2D by restricting to the X/Y plane
      Gizmos.DrawWireSphere(attackPoint.position, attackArea);
      
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, chasingArea);
   }

   
   private void GiveDamage()
   {
      float attackArea = base.attackArea;
      Context.CheckForArea(base.attackPoint.position, attackArea, base
         .detectionLayerMask, out Collider2D obj);

      if (obj.TryGetComponent<BaseHealthScript>(out BaseHealthScript objHealth))
      {
         objHealth.TakeDamage(attackDamage);
      }
      else
      {
         Debug.LogError("Something went wrong, can't find the player");
      }
   }
}
