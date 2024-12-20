using System;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_KnockBackActionNode<TknockBack> : Node where TknockBack : Enum
    {
        private Context knockBackContextRequirements;
        private Vector2 direction;
        private Transform playerTrasform;
        private float maxKnockBackTime;
        private float timer;
        private float knockBackStrength;
        private TknockBack knockBackStateName;
        
        public Enemy_KnockBackActionNode(Context knockBackContextRequirements, Node parent,EnemyHealth enemyHealth,
            float knockBackStrength,TknockBack knockBackStateName)
        {
            this.knockBackStateName = knockBackStateName;
            this.parent = parent;
            this.knockBackContextRequirements = knockBackContextRequirements;
            enemyHealth.onKnockBackTrigger += KnockedBack;
            this.knockBackStrength = knockBackStrength;
            maxKnockBackTime = this.knockBackContextRequirements.animation_VisualsHandler.GetAnimationClipTime(knockBackStateName);
        }

        private void KnockedBack()
        {
            knockBackContextRequirements.isGettingKnockedBack = true;
        }

        public override Node StartNode()
        {
            if (!knockBackContextRequirements.isGettingKnockedBack) return this; // start node in behaviour needs a real change D: 
            GetPlayerTransformReference();
            AdjustKnockBackDirection();
            knockBackContextRequirements.animation_VisualsHandler.SwitchAnimation(knockBackStateName);
            knockBackContextRequirements.rigidbody.AddForce(direction * knockBackStrength,ForceMode2D.Impulse);
            return this;
        }

        public override Status Evaluate()
        {
            timer += Time.deltaTime;
            return timer > maxKnockBackTime ? Status.Success : Status.Running;
        }

        public override void Reset()
        {
            timer = 0;
            knockBackContextRequirements.isGettingKnockedBack = false;
            knockBackContextRequirements.rigidbody.velocity = Vector2.zero;
            ;
        }

        private void GetPlayerTransformReference()
        {
            float chasingArea = knockBackContextRequirements.chasingArea;
            bool canTrack = Context.CheckForArea(knockBackContextRequirements.entityTransform.position, 
                chasingArea, knockBackContextRequirements.desiredDetectionLayer, out Collider2D obj);
            
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
        
        private void AdjustKnockBackDirection()
        {
            if (playerTrasform == null) return;
            direction = (knockBackContextRequirements.entityTransform.position - playerTrasform.position).normalized;
        }
        
    }
}