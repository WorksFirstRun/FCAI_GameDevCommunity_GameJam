using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_IdleActionNode<TIdle> : Node where TIdle : Enum
    {
        private float activeTime;
        private Context idleContextRequirements;
        private float idleTime;
        private Coroutine roamCoroutine;
        private bool randomPointPicked;
        private LayerMask desiredObstacleLayer;
        private TIdle idleStateName;
        
        public Enemy_IdleActionNode(Context idleContextRequirements, Node parent, float activeTime,LayerMask dOL,TIdle idleStateName)
        {
            this.idleStateName = idleStateName;
            randomPointPicked = false;
            this.activeTime = activeTime;
            this.parent = parent;
            this.idleContextRequirements = idleContextRequirements;
            desiredObstacleLayer = dOL;
        }
        
        public override Node StartNode()
        {
            idleContextRequirements.animation_VisualsHandler.SwitchAnimation(idleStateName);
            idleContextRequirements.entityTransform.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(PickAPoint());
            return this;
        }

        public override Status Evaluate()
        {
            idleTime += Time.deltaTime;
            return (idleTime > activeTime && randomPointPicked) ? Status.Success : Status.Running;
        }

        public override void Reset()
        {
            idleContextRequirements.entityTransform.gameObject.GetComponent<MonoBehaviour>().StopAllCoroutines();
            randomPointPicked = false;
            idleTime = 0;
        }
        
        
        private IEnumerator PickAPoint()
        {
            randomPointPicked = false;
            while (!randomPointPicked)
            {
                Vector2 randomPoint = PickRandomPoint();
                
                RaycastHit2D hit = Physics2D.Linecast(idleContextRequirements.entityTransform.position, randomPoint,desiredObstacleLayer);

                if (hit.collider == null)
                {
                    randomPointPicked = true;
                    idleContextRequirements.randomPoint_Roam = randomPoint;
                }
               
                
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        
        private Vector2 PickRandomPoint()
        {
            float randomAngel = Random.Range(0, Mathf.PI * 2);
            float distance = 5f;
            Vector2 randomPoint = new Vector2(Mathf.Cos(randomAngel) * distance + idleContextRequirements.entityTransform.position.x, 
                Mathf.Sin(randomAngel) * distance + idleContextRequirements.entityTransform.position.y);
            return randomPoint;
        }

    }
}