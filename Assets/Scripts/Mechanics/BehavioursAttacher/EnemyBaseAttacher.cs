using BehaviourTreeNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   protected BehaviourTree _behaviourTree;
   [SerializeField] protected Rigidbody2D rb;
   [SerializeField] protected float movementSpeed;
   [SerializeField] protected Transform enemyTransform;
   [SerializeField] protected float chasingArea;
   [SerializeField] protected float attackArea;
   [SerializeField] protected float castSpellArea;
   [SerializeField] protected float attackDamage;
   [SerializeField] protected LayerMask detectionLayerMask;
   [SerializeField] protected LayerMask obstacleLayerMask;
   [SerializeField] protected float idleTime;
   [SerializeField] protected Transform attackPoint;
   [SerializeField] protected float knockBackStrength;
   [SerializeField] protected BaseAnimationAndVisualsScript enemyAnimationVisuals;
   [SerializeField] protected EnemyHealth _enemyHealth;
   
   public void DisableEnemyBehaviour()
   {
      _behaviourTree.DisableTheTree();
   }
}
