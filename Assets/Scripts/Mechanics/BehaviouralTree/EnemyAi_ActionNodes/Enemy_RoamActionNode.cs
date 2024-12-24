using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;
using System;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_RoamActionNode<TRoam> : Node where TRoam : Enum
    {
        private Context roamContextRequirements;
        private Vector2 direction;
        private float pointDistanceThreshHold;
        private bool isReached;
        private float nodeTimeOut;
        private float timer;
        private TRoam roamStateName;
        
        
        public Enemy_RoamActionNode(Context roamContextRequirements, Node parent,TRoam roamStateName)
        {
            this.parent = parent;
            pointDistanceThreshHold = 1.2f;
            this.roamContextRequirements = roamContextRequirements;
            this.roamStateName = roamStateName;
            nodeTimeOut = 2f;
        }

        public override Node StartNode()
        {
            // start running animation here
            direction = new Vector2(
                roamContextRequirements.randomPoint_Roam.x - roamContextRequirements.entityTransform.position.x,
                roamContextRequirements.randomPoint_Roam.y - roamContextRequirements.entityTransform.position.y).normalized;
            roamContextRequirements.animation_VisualsHandler.SwitchAnimation(roamStateName);
            Move();
            roamContextRequirements.animation_VisualsHandler.AdjustVisualDirection(direction.x);
            return this;
        }

        public override Status Evaluate()
        {
            timer += Time.deltaTime;
            CheckIfReached(); 
            return isReached || timer > nodeTimeOut ? Status.Success : Status.Running;
        }

        public override void Reset()
        {
            roamContextRequirements.rigidbody.velocity = Vector2.zero;
            isReached = false;
            timer = 0f;
        }


        private void Move()
        {
            float speed = roamContextRequirements.moveSpeed;
            roamContextRequirements.rigidbody.velocity = direction * speed;
        }
        
        private void CheckIfReached()
        {
            float pointDistance = Mathf.Sqrt(Mathf.Pow(roamContextRequirements.randomPoint_Roam.x - 
                                                       roamContextRequirements.entityTransform.position.x, 2) 
                                             + Mathf.Pow(roamContextRequirements.randomPoint_Roam.y
                                                         - roamContextRequirements.entityTransform.position.y, 2));
            if (pointDistance < pointDistanceThreshHold)
            {
                isReached = true;
            }
        }

    }
}
