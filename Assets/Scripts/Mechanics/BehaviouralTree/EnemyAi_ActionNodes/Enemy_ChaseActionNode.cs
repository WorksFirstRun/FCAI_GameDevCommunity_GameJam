using System;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_ChaseActionNode<TChase> : Node where TChase : Enum
    {
        private Context chaseContextRequirements;
        private Vector2 direction;
        private Transform playerTrasform;
        private TChase chaseStateName;
        private Vector2 oldDirection;
        private float directionThreshold = 0.1f;
        
        public Enemy_ChaseActionNode(Context chaseContextRequirements, Node parent,TChase chaseStateName)
        {
            oldDirection = Vector2.zero;
            this.parent = parent;
            this.chaseContextRequirements = chaseContextRequirements;
            this.chaseStateName = chaseStateName;
        }

        public override Node StartNode()
        {
            chaseContextRequirements.animation_VisualsHandler.SwitchAnimation(chaseStateName);
            GetPlayerTransformReference();
            return this;
        }

        private void AdjustDirectionToThePlayer()
        {
            Vector2 directionT = (playerTrasform.position - chaseContextRequirements.entityTransform.position)
                .normalized;
            if (!(Mathf.Abs(directionT.x) > directionThreshold) ||
                Math.Sign(oldDirection.x) == Math.Sign(directionT.x)) return;
            
            oldDirection = directionT;
            chaseContextRequirements.animation_VisualsHandler.AdjustVisualDirection(directionT.x);
        }
        
        
        public override Status Evaluate()
        {
            AdjustChaseDirection();
            AdjustDirectionToThePlayer();
            Move();
            return Status.Running;
        }

        public override void Reset()
        {
            chaseContextRequirements.rigidbody.velocity = Vector2.zero;
        }

        private void GetPlayerTransformReference()
        {
            float chasingArea = chaseContextRequirements.chasingArea;
            bool canTrack = Context.CheckForArea(chaseContextRequirements.entityTransform.position, 
                chasingArea, chaseContextRequirements.desiredDetectionLayer, out Collider2D obj);
            
            if (!canTrack) return;
            
            if (obj.TryGetComponent<Transform>(out Transform objTransform))
            {
                playerTrasform = objTransform;
            }
            else
            {
                Debug.LogError("Something went wrong, can't find the player");
            }
        }
        
        private void AdjustChaseDirection()
        {
            direction = (playerTrasform.position - chaseContextRequirements.firePoint.position).normalized;
        }

        private void Move()
        {
            float speed = chaseContextRequirements.moveSpeed;
            chaseContextRequirements.rigidbody.velocity = direction * speed;
        }
    }
}