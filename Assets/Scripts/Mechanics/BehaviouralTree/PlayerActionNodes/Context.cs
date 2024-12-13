using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class Context
    {
        public float moveSpeed { get; set; }
        public Rigidbody2D rigidbody { get; set; }
        
        public ZoomIn_OutCamera InOutCamera { get; set; }
        
        public PlayerAnimation_Visuals playerAnimations { get; set; }
        
        public FirePower firePower { get; set; }
        
        public UIBars ui_bars { get; set; }
        
        public Context(float moveSpeed, Rigidbody2D rigidbody,PlayerAnimation_Visuals playerAnimations, 
            ZoomIn_OutCamera ioCamera,FirePower fp,UIBars uiBars)
        {
            this.moveSpeed = moveSpeed;
            this.rigidbody = rigidbody;
            this.playerAnimations = playerAnimations;
            InOutCamera = ioCamera;
            firePower = fp;
            ui_bars = uiBars;
        }
    }
}