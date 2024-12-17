using System;
using BehaviourTreeNamespace;
using BehaviourTreeNamespace.EnemyAi_ActionNodes;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;

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
   
   private void Start()
   {
      Context enemyLeafsContext = new Context
      {
         rigidbody = rb,
         moveSpeed = movementSpeed,
         playerTransform = enemyTransform
      };
      enemyLeafsContext.SetEnemyAreas(chasingArea, attackArea, castSpellArea, detectionLayerMask);


      _behaviourTree = new BehaviourTree();
      Selector selector = new Selector(_behaviourTree);
      
      _behaviourTree.AddChild(selector);
      
      FallBackNode idle_RoamFallBack =
         new FallBackNode(enemyLeafsContext, FallBackNode.CheckForFallBack.Idle_Roaming, selector);

      Sequence idle_roam_sequencer = new Sequence(idle_RoamFallBack);
      
      idle_RoamFallBack.AddChild(idle_roam_sequencer);
      selector.AddChild(idle_RoamFallBack);


      Enemy_IdleActionNode enemyIdleActionNode = new Enemy_IdleActionNode(enemyLeafsContext, idle_roam_sequencer, idleTime,obstacleLayerMask);
      Enemy_RoamActionNode enemyRoamActionNode = new Enemy_RoamActionNode(enemyLeafsContext, idle_roam_sequencer);
      
      idle_roam_sequencer.AddChild(enemyIdleActionNode);
      idle_roam_sequencer.AddChild(enemyRoamActionNode);
   }

   private void Update()
   {
      _behaviourTree.Evaluate();
   }
}
