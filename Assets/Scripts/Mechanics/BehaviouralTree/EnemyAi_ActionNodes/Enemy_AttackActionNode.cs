using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_AttackActionNode : Node
    {
        private Context attackContextRequirements;
        private float maxAttackTime;
        private float waitForNextAttack;
        private float attackTimeCounter;
        private float attackDamage;
        private float firstFrameAttackTime;
        private float secondFrameAttackTime;
        private AttackStates attackState;

        private bool isFirstFrameDone;
        private bool isSecondFrameDone;
        
        private enum AttackStates
        {
            attacking,
            waiting
        }
        
        public Enemy_AttackActionNode(Context roamContextRequirements, Node parent
        ,float waitForNextAttack,float attackDamage)
        {
            attackState = AttackStates.attacking;
            this.parent = parent;
            attackContextRequirements = roamContextRequirements;
            this.waitForNextAttack = waitForNextAttack;
            maxAttackTime = attackContextRequirements.EnemyAnimations.GetAnimationClipTime(EnemyAnimations.Attack);
            attackTimeCounter = 0;
            firstFrameAttackTime = 5f / 60f; 
            secondFrameAttackTime = 21f / 60f; 
            this.attackDamage = attackDamage;
        }

        public override Node StartNode()
        {
            attackContextRequirements.EnemyAnimations.SwitchAnimation(EnemyAnimations.Attack);
            return this;
        }

        public override Status Evaluate()
        {
            attackTimeCounter += Time.deltaTime;
            switch (attackState)
            {
                case AttackStates.waiting:
                    if (attackTimeCounter > waitForNextAttack)
                    {
                        attackTimeCounter = 0;
                        attackState = AttackStates.attacking;
                        attackContextRequirements.EnemyAnimations.SwitchAnimation(EnemyAnimations.Attack);
                    }
                    break;
                case AttackStates.attacking:
                    
                    if (attackTimeCounter > maxAttackTime)
                    {
                        attackContextRequirements.EnemyAnimations.SwitchAnimation(EnemyAnimations.Idle);
                        attackState = AttackStates.waiting;
                        isSecondFrameDone = false;
                        isFirstFrameDone = false;
                        attackTimeCounter = 0;
                    }
                    
                    else if (attackTimeCounter > secondFrameAttackTime && !isSecondFrameDone)
                    {
                        isSecondFrameDone = true;
                        GiveDamage();
                    }
                    
                    else if (attackTimeCounter > firstFrameAttackTime && !isFirstFrameDone)
                    {
                        isFirstFrameDone = true;
                        GiveDamage();
                    }
                    break;
            }
           
            
            return Status.Running;
        }

        public override void Reset()
        {
            attackTimeCounter = 0;
            isFirstFrameDone = false;
            isSecondFrameDone = false;
            attackState = AttackStates.attacking;
        }

        private void GiveDamage()
        {
            float attackArea = attackContextRequirements.attackingArea;
            attackContextRequirements.CheckForArea(attackContextRequirements.firePoint.position, attackArea, attackContextRequirements
                .desiredDetectionLayer
                , out Collider2D obj);

            if (obj.TryGetComponent<BaseHealthScript>(out BaseHealthScript objHealth))
            {
                objHealth.TakeDamage(attackDamage);
            }
            else
            {
                Debug.LogError("Something went wrong, can't find the player");
            }
            
        }
    }
}
