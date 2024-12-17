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
    [SerializeField]
    private PlayerAnimation_Visuals playerAnimationVisuals;
    [SerializeField] private FirePower firePower;
    [SerializeField] private UIBars _uiBars;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObjectRefrence_SO fireballReference;
    
    private void Start()
    {
        Context leafContext = new Context(playerSpeed,rb,playerAnimationVisuals,inOutCamera,firePower,_uiBars,firePoint
        ,fireballReference, playerTransform);
        _behaviourTree = new BehaviourTree();
        Selector selector = new Selector(null);
        _behaviourTree.AddChild(selector);
        
        MoveLeaf moveLeaf = new MoveLeaf(leafContext,selector,1);
        IdleLeaf idleLeaf = new IdleLeaf(leafContext, selector);
        AttackLeaf attackLeaf = new AttackLeaf(leafContext,2,this);
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
}
