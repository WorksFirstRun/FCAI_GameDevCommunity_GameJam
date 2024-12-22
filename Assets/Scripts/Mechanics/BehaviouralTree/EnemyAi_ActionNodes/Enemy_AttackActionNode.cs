using System;
using System.Collections.Generic;
using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_AttackActionNode<TAttack> : Node where TAttack : Enum 
    {
        private Context attackContextRequirements;
        private float maxAttackTime;
        private float waitForNextAttack;
        private float timer;
        private AttackStates attackState;
        private TAttack idleStateName;
        private bool isEndOfFrames;
        private Transform playerRefrence;
        private Vector2 oldDirection;
        private float directionThreshold = 0.1f;
        
        private struct AttackInformation
        {
            public Action attack;
            public float[] attackFrames;
            public TAttack attackName;

            public AttackInformation(Action attack, float[] attackFrames,TAttack attackName)
            {
                this.attack = attack;
                this.attackFrames = attackFrames;
                this.attackName = attackName;
            }
        }

        private List<AttackInformation> attacksInfo;
        private float nextFrameAttackTime;
        private int currentFrameIndex;
        private int currentAttackIndex;
        
        
        private enum AttackStates
        {
            attacking,
            waiting
        }
        
        public Enemy_AttackActionNode(Context roamContextRequirements, Node parent, float waitForNextAttack,Action[] attack,TAttack[] attacksName,TAttack idleStateName)
        {
            attacksInfo = new List<AttackInformation>();
            attackState = AttackStates.waiting;
            this.idleStateName = idleStateName;
            currentAttackIndex = 0;
            this.parent = parent;
            attackContextRequirements = roamContextRequirements;
            this.waitForNextAttack = waitForNextAttack;
            maxAttackTime = attackContextRequirements.animation_VisualsHandler.GetAnimationClipTime(attacksName[currentAttackIndex]);
            timer = 0;

            for (int i = 0 ; i < attacksName.Length ; i++)
            {
                attacksInfo.Add(new AttackInformation
                ( 
                    attack[i],
                    attackContextRequirements.animation_VisualsHandler.GetAnimationFrames(attacksName[i]),
                    attacksName[i]
                ));
            }
            

            nextFrameAttackTime = attacksInfo[currentAttackIndex].attackFrames[0];
            currentFrameIndex = 0;
        }

        public override Node StartNode()
        {
            attackContextRequirements.animation_VisualsHandler.SwitchAnimation(idleStateName);
            float attackArea = attackContextRequirements.attackingArea;
            bool isIn = Context.CheckForArea(attackContextRequirements.firePoint.position, attackArea, attackContextRequirements.desiredDetectionLayer, out Collider2D obj);
            if (!isIn) return this;
            Vector2 direction = (obj.transform.position - attackContextRequirements.entityTransform.position);
            playerRefrence = obj.transform;
            attackContextRequirements.animation_VisualsHandler.AdjustVisualDirection(direction.x);
            return this;
        }

        private void AdjustDirectionToThePlayer()
        {
            Vector2 direction = (playerRefrence.position - attackContextRequirements.entityTransform.position)
                .normalized;
            if (!(Mathf.Abs(direction.x) > directionThreshold) ||
                Math.Sign(oldDirection.x) == Math.Sign(direction.x)) return;
            
            oldDirection = direction;
            attackContextRequirements.animation_VisualsHandler.AdjustVisualDirection(direction.x);
        }
        
        public override Status Evaluate()
        {
            timer += Time.deltaTime;
            AdjustDirectionToThePlayer();
            switch (attackState)
            {
                case AttackStates.waiting:
                    if (timer > waitForNextAttack)
                    {
                        currentAttackIndex = (currentAttackIndex + 1) % attacksInfo.Count;
                        timer = 0;
                        attackState = AttackStates.attacking;
                        attackContextRequirements.animation_VisualsHandler.SwitchAnimation(attacksInfo[currentAttackIndex].attackName);
                        nextFrameAttackTime = attacksInfo[currentAttackIndex].attackFrames[0]; 
                        currentFrameIndex = 0;
                        isEndOfFrames = false;
                        maxAttackTime = attackContextRequirements.animation_VisualsHandler.GetAnimationClipTime(attacksInfo[currentAttackIndex].attackName);
                    }
                    break;

                case AttackStates.attacking:
                    if (timer > maxAttackTime)
                    {
                        attackContextRequirements.animation_VisualsHandler.SwitchAnimation(idleStateName);
                        attackState = AttackStates.waiting;
                        timer = 0;
                        currentFrameIndex = 0;
                    }
                    else if (timer > nextFrameAttackTime && !isEndOfFrames)
                    {
                        attacksInfo[currentAttackIndex].attack?.Invoke();
                       
                        currentFrameIndex++;
                        if (currentFrameIndex < attacksInfo[currentAttackIndex].attackFrames.Length)
                        {
                            nextFrameAttackTime = attacksInfo[currentAttackIndex].attackFrames[currentFrameIndex];
                        }
                        else
                        {
                            isEndOfFrames = true;
                        }
                    }
                    break;
            }

            return Status.Running;
        }

        public override void Reset()
        {
            timer = 0;
            currentFrameIndex = 0;
            isEndOfFrames = false;
            currentAttackIndex = 0;
            attackState = AttackStates.waiting;
        }
        
    }
}
