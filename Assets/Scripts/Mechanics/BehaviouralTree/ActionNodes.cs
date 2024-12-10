using BehaviourTreeNamespace;
using UnityEngine;


namespace ActionNodesNamespace
{
   
    public class Context
    {
        public float moveSpeed { get; set; }
        public Rigidbody2D rigidbody { get; set; }
        
        public Context(float moveSpeed, Rigidbody2D rigidbody)
        {
            this.moveSpeed = moveSpeed;
            this.rigidbody = rigidbody;
        }
    }
    
    
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
            SetCurrentChild(nodeIndex);
        }

        public override Node StartNode()
        {
            return this;
        }

        public override Status Evaluate()
        {
            if (RefreshMovementDirection() == Vector2.zero)
            {
                return Status.Fail;
            }
            MovePlayer(RefreshMovementDirection());
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


    public class IdleLeaf : Node
    {
        private Context idleContextRequirements;

        public override Node StartNode()
        { 
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