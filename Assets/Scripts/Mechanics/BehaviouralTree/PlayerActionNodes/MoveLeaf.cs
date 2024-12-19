using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class MoveLeaf : Node
    {
        private Context moveContextRequirements;
        private float rotationAngel;
        private int nodeIndex;
        
        public MoveLeaf(Context moveContextRequirements,Node parent,int nodeIndex)
        {
            this.moveContextRequirements = moveContextRequirements;
            this.parent = parent;
            InputManager.Instance.onMovementPerformed += MovementPerformed;
            this.nodeIndex = nodeIndex;
        }

        private void MovementPerformed()
        {
            if (GetActiveLeafNode() is not AttackLeaf
                && GetActiveLeafNode() is not DashLeaf
                && GetActiveLeafNode() is not ReleaseSpellLeaf)
            {
                SetCurrentChild(nodeIndex);
            }
        }

        public override Node StartNode()
        {
            moveContextRequirements.playerAnimation.SwitchAnimation(PlayerAnimations.Run);
            return this;
        }

        public override Status Evaluate()
        {
            if (RefreshMovementDirection() == Vector2.zero)
            {
                return Status.Fail;
            }
            MovePlayer(RefreshMovementDirection());
            moveContextRequirements.playerAnimation.AdjustVisualDirection(RefreshMovementDirection().x);
            return Status.Running;
        }
        
        public override void Reset()
        {
            moveContextRequirements.rigidbody.velocity = Vector3.zero;
        }
        
        private void MovePlayer(Vector2 direction)
        {
            moveContextRequirements.rigidbody.velocity = direction * moveContextRequirements.moveSpeed;
        }
        
        private Vector2 RefreshMovementDirection()
        {
            return InputManager.Instance.GetMovementInputDirection_Normalized();
        }
    }
}