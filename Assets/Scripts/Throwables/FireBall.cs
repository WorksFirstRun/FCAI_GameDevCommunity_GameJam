using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private const string EXPLOSION = "Explosion";
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float destroyDelayTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private float fireBallSpeed;
    [SerializeField] private float damageAmount;
    [SerializeField] private bool decreaseTheFirePower;
    [SerializeField] private collidedTags desiredDamageableObject;
    private Transform caster;
    private bool isDamaged;
    

    private enum collidedTags
    {
        Enemy,
        Player
    }
    
    public void InitializeTheFireBall(Vector2 direction,Transform casteTransform)
    {
        caster = casteTransform;
        direction = direction.normalized;
        transform.right = direction;

        rb.velocity = direction * fireBallSpeed;
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == caster || isDamaged) return;
        if (other.CompareTag(desiredDamageableObject.ToString()))
        {
            isDamaged = true;
            if (other.TryGetComponent(out BaseHealthScript otherHealth))
            {
                otherHealth.TakeDamage(damageAmount);
            }
    
            if (decreaseTheFirePower)
            {
                if (other.TryGetComponent(out FirePower otherFireCharge))
                {
                    float df = otherFireCharge.GetDecayingFireFactor();
                    otherFireCharge.DecreaseDecayingFactor(df - 0.20f);
                }
            }
            
        }
        
        rb.velocity = Vector2.zero;
        _animator.Play(EXPLOSION,0);
        Destroy(gameObject,destroyDelayTime);
    }
}
