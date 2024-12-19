using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
   public class ChargeFireLeaf : Node
    {
        private Context chargeFireContextRequirements;
        private Status chargingStatus;
        private float chargingTimer;
        private float maxChargeTime;
        private bool startCounting;
        private int nodeIndex;
        
        public ChargeFireLeaf(Context chargeFireContext,Node p,int nodeIndex)
        {
            chargeFireContextRequirements = chargeFireContext;
            InputManager.Instance.onCastPerformed += FireCharging;
            InputManager.Instance.onCastReleased += FireRelease;
            chargingTimer = 0;
            maxChargeTime = 1f;
            startCounting = false;
            chargingStatus = Status.Fail;
            parent = p;
            this.nodeIndex = nodeIndex;
        }

        ~ChargeFireLeaf()
        {
            InputManager.Instance.onCastReleased -= FireRelease;
            InputManager.Instance.onCastPerformed -= FireCharging;
        }

        public override Node StartNode()
        {
            return this;
        }

        private void FireCharging()
        {
            if (parent.GetActiveLeafNode() is not DashLeaf
                && parent.GetActiveLeafNode() is not AttackLeaf
                /*dont forget to check for the release leaf here */)
            {
                parent.SetCurrentChild(nodeIndex);
                chargingStatus = Status.Running;
                chargingTimer = 0;
                startCounting = true;
                chargeFireContextRequirements.InOutCamera.ZoomIn();
                chargeFireContextRequirements.playerAnimation.SwitchAnimation(PlayerAnimations.ChargeSpell);
                chargeFireContextRequirements.ui_bars.ShowChargeBar();
            }
        }

        private void AdjustVisualsBasedOnAimDirection()
        {
            chargeFireContextRequirements.playerAnimation.AdjustVisualDirection(
                chargeFireContextRequirements.GetMouseDirection().x);
        }
        
        private void FireRelease()
        {
            chargingStatus = chargingTimer >= maxChargeTime ? Status.Success : Status.Fail;
            chargingTimer = 0;
            startCounting = false;
        }
        
        public override Status Evaluate()
        {
            if (startCounting)
            {
                chargingTimer += Time.deltaTime;
                AdjustVisualsBasedOnAimDirection();
                chargeFireContextRequirements.ui_bars.FillChargeBar(chargingTimer);
            }
            return chargingStatus;
        }

        public override void Reset()
        {
            chargingTimer = 0;
            startCounting = false;
            chargingStatus = Status.Fail;
            chargeFireContextRequirements.InOutCamera.ZoomOut();
            chargeFireContextRequirements.ui_bars.HideChargeBar();
        }
    }
}