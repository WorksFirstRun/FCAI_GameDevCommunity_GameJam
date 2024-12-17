using System;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private const string EXPLOSION = "Explosion";
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float destroyDelayTime;
    [SerializeField] private Animator _animator;
    [SerializeField] private float fireBallSpeed;
    private Transform caster;
    
    public void InitializeTheFireBall(Vector2 direction,Transform casteTransform)
    {
        caster = casteTransform;
        direction = direction.normalized;
        transform.right = direction;

        rb.velocity = direction * fireBallSpeed;
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == caster) return;
        // don't forget to check of the health abstract class here and give damage to the object
        rb.velocity = Vector2.zero;
        _animator.SetTrigger(EXPLOSION);
        Destroy(gameObject,destroyDelayTime);
    }
}
