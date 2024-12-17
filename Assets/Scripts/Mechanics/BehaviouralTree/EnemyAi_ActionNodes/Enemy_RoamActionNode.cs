using Mechanics.BehaviouralTree.PlayerActionNodes;
using UnityEngine;
using System.Collections;

namespace BehaviourTreeNamespace.EnemyAi_ActionNodes
{
    public class Enemy_RoamActionNode : Node
    {
        private Context idleContextRequirements;
        private Vector2 direction;
        private Vector2 randomPoint;
        private float pointDistance;
        private float pointDistanceThreshHold;
        private bool isReached;

        private Coroutine roamCoroutine;

        public Enemy_RoamActionNode(Context idleContextRequirements, Node parent, float activeTime, float pointDistanceThreshHold)
        {
            this.parent = parent;
            this.idleContextRequirements = idleContextRequirements;
            this.pointDistanceThreshHold = pointDistanceThreshHold;
        }

        public override Node StartNode()
        {
            return this;
        }

        public override Status Evaluate()
        {
            // Start the roaming coroutine if it hasn't been started
            if (roamCoroutine == null)
            {
                roamCoroutine = idleContextRequirements.playerTransform.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(Roam());
            }

            // We can have the action continue to run until a condition is met (e.g., reached a destination)
            return Status.Running;
        }

        public override void Reset()
        {
            // Stop the coroutine if necessary when the action is reset
            if (roamCoroutine != null)
            {
                idleContextRequirements.playerTransform.gameObject.GetComponent<MonoBehaviour>().StopCoroutine(roamCoroutine);
                roamCoroutine = null;
            }
        }

        private IEnumerator Roam()
        {
            bool randomPointPicked = false;
            while (!randomPointPicked)
            {
                PickRandomPoint();
                
                RaycastHit2D hit = Physics2D.Linecast(idleContextRequirements.playerTransform.position, randomPoint);

                if (hit.collider == null)
                {
                    randomPointPicked = true;
                }
                
                yield return new WaitForSeconds(0.5f); 
            }
            
        }

        private void PickRandomPoint()
        {
            float randomAngel = Random.Range(0, Mathf.PI * 2);
            float distance = 5f;
            randomPoint = new Vector2(Mathf.Cos(randomAngel) * distance + idleContextRequirements.playerTransform.position.x, 
                Mathf.Sin(randomAngel) * distance + idleContextRequirements.playerTransform.position.y);
            direction = new Vector2(randomPoint.x - idleContextRequirements.playerTransform.position.x,
                randomPoint.y - idleContextRequirements.playerTransform.position.y).normalized;
        }

        private void CheckIfReached()
        {
            pointDistance = Mathf.Sqrt(Mathf.Pow(randomPoint.x - idleContextRequirements.playerTransform.position.x, 2) 
                                       + Mathf.Pow(randomPoint.y - idleContextRequirements.playerTransform.position.y, 2));
            if (pointDistance < pointDistanceThreshHold)
            {
                isReached = true;
            }
        }
    }
}
