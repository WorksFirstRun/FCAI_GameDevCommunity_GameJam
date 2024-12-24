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
        private float releaseTime;
        private bool isInstantiated;
        
        public ReleaseSpellLeaf(Context chargeFireContext,Node p,float releaseTime)
        {
            isInstantiated = false;
            releaseSpellLeafRequirments = chargeFireContext;
            chargingStatus = Status.Fail;
            maxNodeActiveTime =
                releaseSpellLeafRequirments.animation_VisualsHandler.GetAnimationClipTime(PlayerAnimations
                    .CastSpell);
            nodeActiveTime = 0;
            parent = p;
            this.releaseTime = releaseTime * 0.0172f; // some factor that gets the actual time
        }

        public override Node StartNode()
        {
            float df = releaseSpellLeafRequirments.firePower.GetDecayingFireFactor();
            if (df >= 0.2)
            {
                releaseSpellLeafRequirments.firePower.DecreaseDecayingFactor(df - 0.15f);
                chargingStatus = Status.Running;
                releaseSpellLeafRequirments.animation_VisualsHandler.SwitchAnimation(PlayerAnimations.CastSpell);
                SoundManager.PlaySound(ClipName.FireBallRelease,releaseSpellLeafRequirments.entityTransform.position);
            }
            else
            {
                SoundManager.PlaySound(ClipName.CantCast,releaseSpellLeafRequirments.entityTransform.position);
                chargingStatus = Status.Fail;
            }
            return this;
        }
        
        public override Status Evaluate()
        {
            nodeActiveTime += Time.deltaTime;
            if (nodeActiveTime >= releaseTime && !isInstantiated)
            {
                isInstantiated = true;
                Transform fb = Object.Instantiate(releaseSpellLeafRequirments.fireBallReference.entity,
                    releaseSpellLeafRequirments.firePoint.position,Quaternion.identity);
                
                fb.GetComponent<FireBall>().InitializeTheFireBall(releaseSpellLeafRequirments.GetMouseDirection(),
                    releaseSpellLeafRequirments.entityTransform);
            }
            
            if (nodeActiveTime > maxNodeActiveTime)
            {
                return Status.Success;
            }
            
            return chargingStatus;
        }

        public override void Reset()
        {
            isInstantiated = false;
            chargingStatus = Status.Fail;
            nodeActiveTime = 0;
        }
    }
}