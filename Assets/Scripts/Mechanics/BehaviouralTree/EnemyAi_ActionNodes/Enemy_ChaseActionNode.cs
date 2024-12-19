using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_ChaseActionNode : Node
    {
        private Context chaseContextRequirements;
        private Vector2 direction;
        private Transform playerTrasform;
        
        public Enemy_ChaseActionNode(Context chaseContextRequirements, Node parent)
        {
            this.parent = parent;
            this.chaseContextRequirements = chaseContextRequirements;
            
        }

        public override Node StartNode()
        {
            chaseContextRequirements.EnemyAnimations.SwitchAnimation(EnemyAnimations.Run);
            GetPlayerTransformReference();
            return this;
        }

        public override Status Evaluate()
        {
            AdjustChaseDirection();
            Move();
            chaseContextRequirements.EnemyAnimations.AdjustVisualDirection(direction.x);
            return Status.Running;
        }

        public override void Reset()
        {
            chaseContextRequirements.rigidbody.velocity = Vector2.zero;
        }

        private void GetPlayerTransformReference()
        {
            float attackArea = chaseContextRequirements.chasingArea;
            bool canTrack = chaseContextRequirements.CheckForArea(chaseContextRequirements.playerTransform.position, 
                attackArea, chaseContextRequirements.desiredDetectionLayer, out Collider2D obj);
            
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