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
        
        public Transform firePoint { get; set; }

        public Transform playerTransform;
        
        public GameObjectRefrence_SO fireBallReference { get; set; }

        public float chasingArea { get; set; }
        
        public float attackingArea { get; set; }
        
        public float castSpellArea { get; set; }
        
        public LayerMask desiredDetectionLayer { get; set; }
        
        public Vector2 randomPoint_Roam { get; set; }
        
        public Context(float moveSpeed, Rigidbody2D rigidbody,PlayerAnimation_Visuals playerAnimations, 
            ZoomIn_OutCamera ioCamera,FirePower fp,UIBars uiBars,Transform firePoint, 
            GameObjectRefrence_SO fireBallReference,Transform playerTransform)
        {
            this.moveSpeed = moveSpeed;
            this.rigidbody = rigidbody;
            this.playerAnimations = playerAnimations;
            InOutCamera = ioCamera;
            firePower = fp;
            ui_bars = uiBars;
            this.firePoint = firePoint;
            this.fireBallReference = fireBallReference;
            this.playerTransform = playerTransform;
        }

        public Context()
        {
            // do nothing 
        }

        public void SetEnemyAreas(float chase, float attack, float cast,LayerMask ddL)
        {
            chasingArea = chase;
            attackingArea = attack;
            castSpellArea = cast;
            desiredDetectionLayer = ddL;
        }
        
        public Vector2 GetMouseDirection()
        {
            Vector3 mouseScreenPosition = Input.mousePosition;

            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));

            Vector2 playerPosition = new Vector2(playerTransform.position.x,
                playerTransform.position.y);

            Vector2 direction = mouseWorldPosition - playerPosition;

            return direction;
        }

        public bool CheckForArea(Vector2 position,float area, LayerMask ddl,out Collider2D obj)
        {
            obj = Physics2D.OverlapCircle(position, area, ddl);

            if (obj == null) return false;

            return true;
        }
        
        public bool CheckForArea(Vector2 position,float area, LayerMask ddl)
        {
            Collider2D obj = Physics2D.OverlapCircle(position, area, ddl);

            if (obj == null) return false;

            return true;
        }
        
        
    }
}