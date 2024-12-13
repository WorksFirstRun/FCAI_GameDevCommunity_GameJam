using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
   public class ReleaseSpellLeaf : Node
    {
        private Context releaseSpellLeafRequirments;
        private Status chargingStatus;
        private float maxNodeActiveTime;
        private float nodeActiveTime;
        
        public ReleaseSpellLeaf(Context chargeFireContext,Node p)
        {
            releaseSpellLeafRequirments = chargeFireContext;
            chargingStatus = Status.Fail;
            maxNodeActiveTime =
                releaseSpellLeafRequirments.playerAnimations.GetAnimationClipTime(PlayerAnimation_Visuals.Animations
                    .CastSpell);
            nodeActiveTime = 0;
            parent = p;
        }

        public override Node StartNode()
        {
            float df = releaseSpellLeafRequirments.firePower.GetDecayingFireFactor();
            if (df >= 0.2)
            {
                releaseSpellLeafRequirments.firePower.DecreaseDecayingFactor(df - 0.15f);
                chargingStatus = Status.Running;
                releaseSpellLeafRequirments.playerAnimations.SwitchAnimation(PlayerAnimation_Visuals.Animations.CastSpell);
            }
            else
            {
                chargingStatus = Status.Fail;
            }
            return this;
        }
        
        
        public override Status Evaluate()
        {
            nodeActiveTime += Time.deltaTime;
            if (nodeActiveTime > maxNodeActiveTime)
            {
                return Status.Success;
            }
            
            return chargingStatus;
        }

        public override void Reset()
        {
            chargingStatus = Status.Fail;
            nodeActiveTime = 0;
        }
    }
}