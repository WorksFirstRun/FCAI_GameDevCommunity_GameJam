using System;
using BehaviourTreeNamespace;
using BehaviourTreeNamespace.EnemyAi_ActionNodes;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
   private BehaviourTree _behaviourTree;
   [SerializeField] private Rigidbody2D rb;
   [SerializeField] private float movementSpeed;
   [SerializeField] private Transform enemyTransform;
   [SerializeField] private float chasingArea;
   [SerializeField] private float attackArea;
   [SerializeField] private float castSpellArea;
   [SerializeField] private LayerMask detectionLayerMask;
   [SerializeField] private LayerMask obstacleLayerMask;
   [SerializeField] private float idleTime;
   [SerializeField] private Transform attackPoint;
   [SerializeField] private float attackDamage;
   [FormerlySerializedAs("darkSamuraiAnimation")] [SerializeField] private EnemyAnimationsVisuals enemyAnimationsVisuals;
   private void Start()
   {
      Context enemyLeafsContext = new Context
      {
         rigidbody = rb,
         moveSpeed = movementSpeed,
         playerTransform = enemyTransform,
         firePoint = attackPoint,
         EnemyAnimations = enemyAnimationsVisuals
      };
      enemyLeafsContext.SetEnemyAreas(chasingArea, attackArea, castSpellArea, detectionLayerMask);


      _behaviourTree = new BehaviourTree();
      Selector selector = new Selector(_behaviourTree);
      
      _behaviourTree.AddChild(selector);
      
      FallBackNode idle_RoamFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Idle_Roaming, selector);
      FallBackNode attackFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Attack, selector);
      FallBackNode chaseFallBack = new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Chase, selector);

      Sequence idle_roam_sequencer = new Sequence(idle_RoamFallBack);
      
      Enemy_IdleActionNode enemyIdleActionNode = new Enemy_IdleActionNode(enemyLeafsContext, idle_roam_sequencer, idleTime,obstacleLayerMask);
      Enemy_RoamActionNode enemyRoamActionNode = new Enemy_RoamActionNode(enemyLeafsContext, idle_roam_sequencer);
      Enemy_AttackActionNode enemyAttackActionNode = new Enemy_AttackActionNode(enemyLeafsContext, attackFallBack, 
         1f, attackDamage);
      Enemy_ChaseActionNode enemyChaseActionNode = new Enemy_ChaseActionNode(enemyLeafsContext, chaseFallBack);
      
      attackFallBack.AddChild(enemyAttackActionNode); // fall backs
      idle_RoamFallBack.AddChild(idle_roam_sequencer); // fall backs
      chaseFallBack.AddChild(enemyChaseActionNode);
      
      idle_roam_sequencer.AddChild(enemyIdleActionNode); // extra 
      idle_roam_sequencer.AddChild(enemyRoamActionNode);
      
      
      selector.AddChild(idle_RoamFallBack); // attach fall backs to selector
      selector.AddChild(attackFallBack);
      selector.AddChild(chaseFallBack);
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

   public void DisableEnemyBehaviour()
   {
      _behaviourTree.DisableTheTree();
   }
   
   
}
