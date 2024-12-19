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
            Cast
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
                case CheckForFallBack.Cast:
                    Debug.LogError("Cast is not Initialized for now");
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
                fallBackRequirments.CheckForArea(fallBackRequirments.playerTransform.position,
                    chaseArea, fallBackRequirments.desiredDetectionLayer) || 
                fallBackRequirments.CheckForArea(fallBackRequirments.playerTransform.position,
                    castArea, fallBackRequirments.desiredDetectionLayer) ||
                fallBackRequirments.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer);

            if (whatToDo)
            {
                fallBack = Status.Fail;
            }
            else
            {
                fallBack = Status.Running;
            }
        }

        void CheckChaseFallBack()
        {
            float attackArea = fallBackRequirments.attackingArea;
            float chaseArea = fallBackRequirments.chasingArea;

            bool whatToDo = 
                ! fallBackRequirments.CheckForArea(fallBackRequirments.playerTransform.position,
                    chaseArea, fallBackRequirments.desiredDetectionLayer) || 
                fallBackRequirments.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer);

            if (whatToDo)
            {
                fallBack = Status.Fail;
            }
            else
            {
                fallBack = Status.Running;
            }
        }

        void CheckAttackFallBack()
        {
            float attackArea = fallBackRequirments.attackingArea;

            bool whatToDo = 
                ! fallBackRequirments.CheckForArea(fallBackRequirments.firePoint.position,
                    attackArea, fallBackRequirments.desiredDetectionLayer);

            if (whatToDo)
            {
                fallBack = Status.Fail;
            }
            else
            {
                fallBack = Status.Running;
            }
        }
    }
}