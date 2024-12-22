using System.Collections.Generic;
using BehaviourTreeNamespace;
using BehaviourTreeNamespace.EnemyAi_ActionNodes;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;


public class FireDemon : Enemy
{
   [SerializeField] private Transform currentRoom;
   [SerializeField] private List<Transform> rooms;
   
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
      
      FallBackNode idle_RoamFallBack = 
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Idle_Roaming, selector);
      FallBackNode knockBackFallBack =
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.KnockBack, selector);
      FallBackNode teleportingFallBackNode =
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Teleporting, selector);
      
      Sequence idle_roam_sequencer = new Sequence(idle_RoamFallBack);
      
      Enemy_IdleActionNode<FireDemonAnimations> enemyIdleActionNode = 
         new Enemy_IdleActionNode<FireDemonAnimations>(enemyLeafsContext, idle_roam_sequencer, 
         idleTime,obstacleLayerMask,FireDemonAnimations.Idle);
      
      
      Enemy_RoamActionNode<FireDemonAnimations> enemyRoamActionNode = 
      new Enemy_RoamActionNode<FireDemonAnimations>(enemyLeafsContext, idle_roam_sequencer,FireDemonAnimations.Run);
      
      
      Enemy_KnockBackActionNode<FireDemonAnimations> enemyKnockBackActionNode = 
      new Enemy_KnockBackActionNode<FireDemonAnimations>(enemyLeafsContext, knockBackFallBack,
      _enemyHealth, knockBackStrength,FireDemonAnimations.Hit);
      
      BossHealth bossHealth = _enemyHealth as BossHealth;
      Enemy_TeleportActionNode<FireDemonAnimations> enemyTeleportActionNode =
         new Enemy_TeleportActionNode<FireDemonAnimations>(enemyLeafsContext, teleportingFallBackNode, currentRoom,
            rooms, bossHealth, FireDemonAnimations.Teleport);
      
      idle_RoamFallBack.AddChild(idle_roam_sequencer); 
      knockBackFallBack.AddChild(enemyKnockBackActionNode);
      teleportingFallBackNode.AddChild(enemyTeleportActionNode);
      
      idle_roam_sequencer.AddChild(enemyIdleActionNode); 
      idle_roam_sequencer.AddChild(enemyRoamActionNode);
      
      selector.AddChild(idle_RoamFallBack);
      selector.AddChild(knockBackFallBack);
      selector.AddChild(teleportingFallBackNode);
      
   }

   private void Update()
   {
      _behaviourTree.Evaluate();
   }
   
 
   
}
