using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class FallBackNode : Node
    {
        private Status fallBack = Status.Running;
        private Context fallBackRequirments;
        private CheckForFallBack fallBackMode;
        public enum CheckForFallBack
        {
            Idle_Roaming,
            Attack,
            Chase,
            KnockBack,
            Teleporting
        }
        
        public FallBackNode(Context fbR,CheckForFallBack fallBackMode,Node parent)
        {
            fallBackRequirments = fbR;
            this.fallBackMode = fallBackMode;
            this.parent = parent;
        }
        
        public override Status Evaluate()
        {
            switch (fallBackMode)
            {
                case CheckForFallBack.Idle_Roaming:
                    Check_Idle_Roam_FallBack();
                    break;
                case CheckForFallBack.Chase:
                    CheckChaseFallBack();
                    break;
                case CheckForFallBack.Attack:
                    CheckAttackFallBack();
                    break;
                case CheckForFallBack.KnockBack:
                    CheckForKnockBackFallBack();
                    break;
                case CheckForFallBack.Teleporting:
                    CheckForTeleportingFallBack();
                    break;
                default:
                    Debug.LogError("wrong FallBack Mode");
                    break;
            }
            
            return fallBack == Status.Running ? childern[currentChild].Evaluate() : Status.Fail;
        }

        void Check_Idle_Roam_FallBack()
        {
            float attackArea = fallBackRequirments.attackingArea;
            float chaseArea = fallBackRequirments.chasingArea;
            float castArea = fallBackRequirments.castSpellArea;

            bool whatToDo =
                Context.CheckForArea(fallBackRequirments.entityTransform.position,
                    chaseArea, fallBackRequirments.desiredDetectionLayer) ||
                Context.CheckForArea(fallBackRequirments.entityTransform.position,
                    castArea, fallBackRequirments.desiredDetectionLayer) ||
                Context.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer) ||
                fallBackRequirments.isGettingKnockedBack ||
                fallBackRequirments.isTeleporting;

            fallBack = whatToDo ? Status.Fail : Status.Running;
        }

        void CheckChaseFallBack()
        {
            float attackArea = fallBackRequirments.attackingArea;
            float chaseArea = fallBackRequirments.chasingArea;

            bool whatToDo = 
                ! Context.CheckForArea(fallBackRequirments.entityTransform.position,
                    chaseArea, fallBackRequirments.desiredDetectionLayer) || 
                Context.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer) ||
                fallBackRequirments.isGettingKnockedBack;

            fallBack = whatToDo ? Status.Fail : Status.Running;
        }

        void CheckAttackFallBack()
        {
            float attackArea = fallBackRequirments.attackingArea;

            bool whatToDo = 
                !Context.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer) ||
                fallBackRequirments.isGettingKnockedBack;

            fallBack = whatToDo ? Status.Fail : Status.Running;
        }

        void CheckForKnockBackFallBack()
        {
            bool whatToDo = fallBackRequirments.isGettingKnockedBack && !fallBackRequirments.isTeleporting;

            fallBack = whatToDo ? Status.Running : Status.Fail;
        }

        void CheckForTeleportingFallBack()
        {
            bool whatToDo = fallBackRequirments.isTeleporting;

            fallBack = whatToDo ? Status.Running : Status.Fail;
        }
    }
}