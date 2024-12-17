using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_IdleActionNode : Node
    {
        private float activeTime;
        private Context idleContextRequirements;
        private float idleTime;


        public Enemy_IdleActionNode(Context idleContextRequirements, Node parent, float activeTime)
        {
            this.activeTime = activeTime;
            this.parent = parent;
            this.idleContextRequirements = idleContextRequirements;
        }
        
        public override Node StartNode()
        {
            return this;
        }

        public override Status Evaluate()
        {
            activeTime += Time.deltaTime;
            return activeTime > idleTime ? Status.Success : Status.Running;
        }

        public override void Reset()
        {
            idleTime = 0;
        }
    }
}