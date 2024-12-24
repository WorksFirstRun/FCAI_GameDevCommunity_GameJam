using BehaviourTreeNamespace;
using BehaviourTreeNamespace.EnemyAi_ActionNodes;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System;

public class WaterWitch : Enemy
{
   [SerializeField] private GameObjectRefrence_SO waterBall;
   [SerializeField] private GameObjectRefrence_SO waterTrap;
   
   public override void InitializeTheEnemy()
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
      FallBackNode knockBackFallBack =
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.KnockBack, selector);
      Sequence idle_roam_sequencer = new Sequence(idle_RoamFallBack);
      
      Enemy_IdleActionNode<WaterWitchAnimation> enemyIdleActionNode = new 
      Enemy_IdleActionNode<WaterWitchAnimation>(enemyLeafsContext, idle_roam_sequencer, idleTime,obstacleLayerMask,WaterWitchAnimation.Idle);
      
      Enemy_RoamActionNode<WaterWitchAnimation> enemyRoamActionNode = 
      new Enemy_RoamActionNode<WaterWitchAnimation>(enemyLeafsContext, idle_roam_sequencer,WaterWitchAnimation.Run);
      
      Action[] attacks = new Action[]
      {
         CastWaterBallSpell,
         CastWaterTrap
      };
      
      WaterWitchAnimation[] attacksName = new WaterWitchAnimation[]
      {
         WaterWitchAnimation.ChargeSpell,
         WaterWitchAnimation.WaterGroundAttack
      };
      
      Enemy_AttackActionNode<WaterWitchAnimation> enemyAttackActionNode = 
      new Enemy_AttackActionNode<WaterWitchAnimation>(enemyLeafsContext, attackFallBack, 
      1.5f, attacks,attacksName,WaterWitchAnimation.Idle);
      
      
      Enemy_KnockBackActionNode<WaterWitchAnimation> enemyKnockBackActionNode = 
      new Enemy_KnockBackActionNode<WaterWitchAnimation>(enemyLeafsContext, knockBackFallBack,
      _enemyHealth, knockBackStrength,WaterWitchAnimation.Hit);

      attackFallBack.AddChild(enemyAttackActionNode);
      idle_RoamFallBack.AddChild(idle_roam_sequencer); // fall backs
      knockBackFallBack.AddChild(enemyKnockBackActionNode);
      
      idle_roam_sequencer.AddChild(enemyIdleActionNode); // extra 
      idle_roam_sequencer.AddChild(enemyRoamActionNode);
      
      
      selector.AddChild(idle_RoamFallBack); // attach fall backs to selector
      selector.AddChild(attackFallBack);
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



   private void CastWaterBallSpell()
   {
      float attackArea = base.attackArea;
      Context.CheckForArea(base.attackPoint.position, attackArea, base
         .detectionLayerMask, out Collider2D obj);

      Vector2 direction = (obj.transform.position - base.attackPoint.position);
      
      Transform fb = Instantiate(waterBall.entity,
         attackPoint.position,Quaternion.identity);
      
      fb.GetComponent<FireBall>().InitializeTheFireBall(direction,
         transform);
   }


   private void CastWaterTrap()
   {
      float attackArea = base.attackArea;
      Context.CheckForArea(base.attackPoint.position, attackArea, base
         .detectionLayerMask, out Collider2D obj);

      Vector2 spawnPosition = new Vector2(obj.transform.position.x, obj.transform.position.y - 1);
      Transform fb = Instantiate(waterTrap.entity,
         spawnPosition,Quaternion.identity);
      
      fb.GetComponent<WaterTrap>().InitializeTheLake(transform);
   }
   
}
