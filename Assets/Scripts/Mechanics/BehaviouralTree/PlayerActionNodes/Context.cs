using UnityEngine;

namespace Mechanics.BehaviouralTree.PlayerActionNodes
{
    public class Context
    {
        public float moveSpeed { get; set; }
        public Rigidbody2D rigidbody { get; set; }
        
        public ZoomIn_OutCamera InOutCamera { get; set; }
        
        public BaseAnimationAndVisualsScript animation_VisualsHandler { get; set; }
        
        public FirePower firePower { get; set; }
        
        public UIBars ui_bars { get; set; }
        
        public Transform firePoint { get; set; }

        public Transform entityTransform;
        
        public GameObjectRefrence_SO fireBallReference { get; set; }

        public float chasingArea { get; set; }
        
        public float attackingArea { get; set; }
        
        public float castSpellArea { get; set; }
        
        public LayerMask desiredDetectionLayer { get; set; }
        
        public Vector2 randomPoint_Roam { get; set; }
        
        public bool isGettingKnockedBack { get; set; }
        
        public bool isTeleporting { get; set; }
        
        public Context(float moveSpeed, Rigidbody2D rigidbody,PlayerAnimation animationVisualsHandler, 
            ZoomIn_OutCamera ioCamera,FirePower fp,UIBars uiBars,Transform firePoint, 
            GameObjectRefrence_SO fireBallReference,Transform entityTransform)
        {
            this.moveSpeed = moveSpeed;
            this.rigidbody = rigidbody;
            this.animation_VisualsHandler = animationVisualsHandler;
            InOutCamera = ioCamera;
            firePower = fp;
            ui_bars = uiBars;
            this.firePoint = firePoint;
            this.fireBallReference = fireBallReference;
            this.entityTransform = entityTransform;
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

            Vector2 playerPosition = new Vector2(entityTransform.position.x,
                entityTransform.position.y);

            Vector2 direction = mouseWorldPosition - playerPosition;

            return direction;
        }

        public static bool CheckForArea(Vector2 position,float area, LayerMask ddl,out Collider2D obj)
        {
            obj = Physics2D.OverlapCircle(position, area, ddl);

            if (obj == null) return false;

            return true;
        }
        
        public static bool CheckForArea(Vector2 position,float area, LayerMask ddl)
        {
            Collider2D obj = Physics2D.OverlapCircle(position, area, ddl);

            if (obj == null) return false;

            return true;
        }
        
        
    }
}