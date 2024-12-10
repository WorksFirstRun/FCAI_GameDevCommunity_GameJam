using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTreeNamespace;
using ActionNodesNamespace;

public class Player : MonoBehaviour
{
    private BehaviourTree _behaviourTree;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float playerSpeed;
    
    private void Start()
    {
        Context leafContext = new Context(playerSpeed,rb);
        _behaviourTree = new BehaviourTree();
        Selector selector = new Selector(null); // will fix this issue later since using setCurrentChildIndex require me to set the parent with null
        _behaviourTree.AddChild(selector);

        MoveLeaf moveLeaf = new MoveLeaf(leafContext,selector,1);
        IdleLeaf idleLeaf = new IdleLeaf(leafContext, selector);

        selector.AddChild(idleLeaf);
        selector.AddChild(moveLeaf);
    }

    private void Update()
    {
        _behaviourTree.Evaluate();
    }
}
