using BehaviourTreeNamespace;
using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    
    public class IdleLeaf : Node
    {
        private Context idleContextRequirements;

        public override Node StartNode()
        { 
            idleContextRequirements.playerAnimations.SwitchAnimation(PlayerAnimation_Visuals.Animations.Idle);
            return this;
        }

        
        public IdleLeaf(Context c,Node parent)
        {
            idleContextRequirements = c;
            this.parent = parent;
        }
        
        public override Status Evaluate()
        {
            
            if (InputManager.Instance.GetMovementInputDirection_Normalized() == Vector2.zero)
            {
                return Status.Running;
            }
            
            return Status.Fail;
        }
        
    }

}