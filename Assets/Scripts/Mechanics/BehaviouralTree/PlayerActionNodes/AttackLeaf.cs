using System.Collections;
using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class AttackLeaf : Node
    {
        private Context attack1ContextRequirments;
        private int nodeIndex;
        private float nodeMaxActiveTime;
        private float [] attackTiming;
        private PlayerAnimation_Visuals.Animations[] _animationsArray;
        private int currentAttack;
        private float nodeActiveTime;
        private bool isActive;

        private MonoBehaviour CoroutineAccessor;

        public AttackLeaf(Context c, int nodeIndex,MonoBehaviour p)
        {
            CoroutineAccessor = p;
            attack1ContextRequirments = c;
            _animationsArray = new PlayerAnimation_Visuals.Animations[]
            {
                PlayerAnimation_Visuals.Animations.Attack1,
                PlayerAnimation_Visuals.Animations.Attack2,
                PlayerAnimation_Visuals.Animations.Attack3
            };
            
             attackTiming = new float[] 
             {  attack1ContextRequirments.playerAnimations.GetAnimationClipTime(PlayerAnimation_Visuals.Animations.Attack1),
                 attack1ContextRequirments.playerAnimations.GetAnimationClipTime(PlayerAnimation_Visuals.Animations.Attack2),
                 attack1ContextRequirments.playerAnimations.GetAnimationClipTime(PlayerAnimation_Visuals.Animations.Attack3)
             };
             
            this.nodeIndex = nodeIndex;
            currentAttack = 0;
            nodeMaxActiveTime = attackTiming[currentAttack];
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
                nodeMaxActiveTime = attackTiming[currentAttack];
                attack1ContextRequirments.playerAnimations.SwitchAnimation(_animationsArray[currentAttack]);
                currentAttack = (currentAttack + 1) % attackTiming.Length;
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
        }

        IEnumerator ResetAttack()
        {
            yield return new WaitForSeconds(2f);
            currentAttack = 0;
        }
    }
}