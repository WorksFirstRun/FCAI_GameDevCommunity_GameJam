﻿using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class DashLeaf : Node
    {
        private Context dashContextRequirments;
        private int nodeIndex;
        private float dashPower;
        private float dashMaxTime;
        private float activeDashTime;
        private Vector2 dashDirection;
        private bool isActive;

        public DashLeaf(Context c, int nI,float dashPower)
        {
            nodeIndex = nI;
            isActive = false;
            dashContextRequirments = c;
            this.dashPower = dashPower;
            dashMaxTime =
                dashContextRequirments.animation_VisualsHandler.GetAnimationClipTime(PlayerAnimations.Dash);
            activeDashTime = 0;
            InputManager.Instance.onDashPerformed += DashPerformed;
        }


        public override Node StartNode()
        {
            if (!isActive) return this;
            
            float df = dashContextRequirments.firePower.GetDecayingFireFactor();
            
            dashContextRequirments.firePower.DecreaseDecayingFactor(df - 0.2f);
            dashContextRequirments.animation_VisualsHandler.SwitchAnimation(PlayerAnimations.Dash);
            dashDirection = InputManager.Instance.GetMovementInputDirection_Normalized();
            dashContextRequirments.rigidbody.AddForce(dashDirection * dashPower,ForceMode2D.Impulse);
            SoundManager.PlaySound(ClipName.Dash,dashContextRequirments.entityTransform.position);
            
            return this;
        }

        private void DashPerformed()
        {
            if (GetActiveLeafNode() is AttackLeaf || GetActiveLeafNode() is IdleLeaf || GetActiveLeafNode() is DashLeaf) return;
            float df = dashContextRequirments.firePower.GetDecayingFireFactor();
            if (df < 0.2f)
            {
                SoundManager.PlaySound(ClipName.CantCast,dashContextRequirments.entityTransform.position);
                return;
            }
            isActive = true;
            SetCurrentChild(nodeIndex);
        }

        public override Status Evaluate()
        {
            if (!isActive) return Status.Fail;
            
            activeDashTime += Time.deltaTime;

            if (activeDashTime > dashMaxTime)
            {
                isActive = false;
                return Status.Success;
            }
            return Status.Running;
        }
        
        
        public override void Reset()
        {
            activeDashTime = 0;
        }
    }
    
}