using System;
using UnityEngine;
using BehaviourTreeNamespace;
using Mechanics.BehaviouralTree.PlayerActionNodes;

public class Player : MonoBehaviour
{
    private BehaviourTree _behaviourTree;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float dashStrength;
    [SerializeField] private ZoomIn_OutCamera inOutCamera;
    [SerializeField] private PlayerAnimation animationAndVisualsScript;
    [SerializeField] private FirePower firePower;
    [SerializeField] private UIBars _uiBars;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform[] attackPoints;
    [SerializeField] private float[] attackAreas;
    private Action[] attacksList;
    [SerializeField] private float [] attacksDamage;
    [SerializeField] private LayerMask desiredDetectionLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObjectRefrence_SO fireballReference;
    
    private void Start()
    {
        Context leafContext = new Context(playerSpeed,rb,animationAndVisualsScript,inOutCamera,firePower,_uiBars,firePoint
        ,fireballReference, playerTransform);
        attacksList = new Action[]
        {
            Attack1,
            Attack2,
            Attack3
        };
        
        _behaviourTree = new BehaviourTree();
        Selector selector = new Selector(null);
        _behaviourTree.AddChild(selector);
        MoveLeaf moveLeaf = new MoveLeaf(leafContext,selector,1);
        IdleLeaf idleLeaf = new IdleLeaf(leafContext, selector);
        AttackLeaf attackLeaf = new AttackLeaf(leafContext,2,this, attacksList);
        DashLeaf dashLeaf = new DashLeaf(leafContext, 3, dashStrength);
        Sequence sequencer = new Sequence(selector);
        
        ChargeFireLeaf chargeFireLeaf = new ChargeFireLeaf(leafContext, sequencer, 4);
        ReleaseSpellLeaf releaseSpellLeaf = new ReleaseSpellLeaf(leafContext, sequencer,22);
        
        sequencer.AddChild(chargeFireLeaf);
        sequencer.AddChild(releaseSpellLeaf);
        
        attackLeaf.SetParent(selector); // redundant, just add it in the constructor 
        dashLeaf.SetParent(selector); // redundant, just add it in the constructor
        
        selector.AddChild(idleLeaf);
        selector.AddChild(moveLeaf);
        selector.AddChild(attackLeaf);
        selector.AddChild(dashLeaf);
        selector.AddChild(sequencer);
        
    }

    private void Update()
    {
        _behaviourTree.Evaluate();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // Draw a wireframe circle/sphere in 2D by restricting to the X/Y plane
        Gizmos.DrawWireSphere(attackPoints[0].position, attackAreas[0]);
      
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoints[1].position, attackAreas[1]);
        
        Gizmos.color = Color.yellow;

        // Replace this with the sword's actual position

        // Radius of the circle (this can be the width and height)
        float radiusY = 0.1f; // Vertical radius (adjust as needed)

        // Store the original Gizmos matrix
        Matrix4x4 originalMatrix = Gizmos.matrix;

        // Apply a scaling transformation to stretch vertically
        Gizmos.matrix = Matrix4x4.TRS(attackPoints[2].position, Quaternion.identity, new Vector3(attackAreas[2], radiusY, 1));

        // Draw an ellipse using a wire sphere
        Gizmos.DrawWireSphere(Vector3.zero, 1);

        // Restore the original Gizmos matrix
        Gizmos.matrix = originalMatrix;
    }

    private void Attack1()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(attackPoints[0].position, attackAreas[0], desiredDetectionLayer);

        if (objectsInRange.Length == 0) return;

        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.TryGetComponent(out BaseHealthScript objHealth))
            {
                objHealth.TakeDamage(attacksDamage[0]);
            }
        }
    }

    private void Attack2()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(attackPoints[1].position, attackAreas[1], desiredDetectionLayer);

        if (objectsInRange.Length == 0) return;

        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.TryGetComponent(out BaseHealthScript objHealth))
            {
                objHealth.TakeDamage(attacksDamage[1]);
            }
        }
    }

    private void Attack3()
    {
        float height = 0.1f; 

        Collider2D[] objectsInRange = Physics2D.OverlapBoxAll(attackPoints[2].position, new Vector2(attackAreas[2], height), 0f, desiredDetectionLayer);

        if (objectsInRange.Length == 0) return;

        foreach (Collider2D obj in objectsInRange)
        {
            if (obj.TryGetComponent(out BaseHealthScript objHealth))
            {
                objHealth.TakeDamage(attacksDamage[2]);
            }
        }
    }

}
