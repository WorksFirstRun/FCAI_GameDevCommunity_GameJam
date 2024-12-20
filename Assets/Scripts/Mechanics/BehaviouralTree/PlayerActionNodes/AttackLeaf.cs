using System.Collections;
using System;
using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class AttackLeaf : Node
    {
        private struct AttackInformation
        {
            public float attackTiming;
            public PlayerAnimations animation;
            public float hitFrame;
            public Action attack;
            
            public AttackInformation(float attackTiming, PlayerAnimations animation, float hitFrame,Action attack)
            {
                this.attackTiming = attackTiming;
                this.animation = animation;
                this.hitFrame = hitFrame;
                this.attack = attack;
            }
        }

        private Context attack1ContextRequirments;
        private int nodeIndex;
        private float nodeMaxActiveTime;
        private AttackInformation[] attackInformationArray; // Single array for all attack data
        private Action currentAttack;
        private int currentAttackNumber;
        private float currentAttackFrame;
        private float nodeActiveTime;
        private bool isActive;
        private bool isHit;

        private MonoBehaviour CoroutineAccessor;

        public AttackLeaf(Context c, int nodeIndex, MonoBehaviour p,Action [] attackPoints)
        {
            CoroutineAccessor = p;
            attack1ContextRequirments = c;

            attackInformationArray = new AttackInformation[]
            {
                new AttackInformation(
                    attack1ContextRequirments.animation_VisualsHandler.GetAnimationClipTime(PlayerAnimations.Attack1),
                    PlayerAnimations.Attack1,
                    7f / 60f,
                    attackPoints[0]
                ),
                new AttackInformation(
                    attack1ContextRequirments.animation_VisualsHandler.GetAnimationClipTime(PlayerAnimations.Attack2),
                    PlayerAnimations.Attack2,
                    7f / 60f,
                    attackPoints[1]
                ),
                new AttackInformation(
                    attack1ContextRequirments.animation_VisualsHandler.GetAnimationClipTime(PlayerAnimations.Attack3),
                    PlayerAnimations.Attack3,
                    14f / 60f,
                    attackPoints[2]
                )
            };

            this.nodeIndex = nodeIndex;
            currentAttackNumber = 0;
            nodeMaxActiveTime = attackInformationArray[currentAttackNumber].attackTiming;
            InputManager.Instance.onAttackPerformed += AttackPerformed;
        }

        private void AttackPerformed()
        {
            if (GetActiveLeafNode() is not AttackLeaf
                && GetActiveLeafNode() is not DashLeaf)
            {
                CoroutineAccessor.StopAllCoroutines();
                SetCurrentChild(nodeIndex);
                isActive = true;
                nodeMaxActiveTime = attackInformationArray[currentAttackNumber].attackTiming;
                attack1ContextRequirments.animation_VisualsHandler.SwitchAnimation(attackInformationArray[currentAttackNumber].animation);
                currentAttackFrame = attackInformationArray[currentAttackNumber].hitFrame;
                currentAttack = attackInformationArray[currentAttackNumber].attack;
                currentAttackNumber = (currentAttackNumber + 1) % attackInformationArray.Length;
            }
        }

        public override Node StartNode()
        {
            return this;
        }

        public override Status Evaluate()
        {
            if (!isActive)
            {
                return Status.Fail;
            }
            nodeActiveTime += Time.deltaTime;
            if (nodeActiveTime > currentAttackFrame && !isHit)
            {
                currentAttack.Invoke();
                isHit = true;
            }
            if (nodeActiveTime > nodeMaxActiveTime)
            {
                CoroutineAccessor.StartCoroutine(ResetAttack());
                return Status.Success;
            }

            return Status.Running;
        }

        public override void Reset()
        {
            isActive = false;
            nodeActiveTime = 0;
            isHit = false;
        }

        IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(2f);
            currentAttackNumber = 0;
            currentAttack = attackInformationArray[currentAttackNumber].attack;
        }
    }
}
