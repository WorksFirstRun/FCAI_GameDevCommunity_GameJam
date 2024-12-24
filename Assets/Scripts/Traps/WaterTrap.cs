
using System;
using System.Collections;
using UnityEngine;

public class WaterTrap : MonoBehaviour
{

    private const string POPUPTHEWATER = "PopUPTheWatter";
    [SerializeField] private float damageAmount;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider2D waterCollider;
    [SerializeField] private float explodeTime;
    [SerializeField] private float destroyTime;
    [SerializeField] private float disableCollidedTime;
    [SerializeField] private CollidedObject desiredDamageableObject;
    private bool isDamaged;
    private Transform caster;

    private enum CollidedObject
    {
        Player,
        Enemy
    }
    
    public void InitializeTheLake(Transform caster)
    {
        waterCollider.enabled = false;
        this.caster = caster;
        StartCoroutine(ExplodeTimer());
    }


    private IEnumerator ExplodeTimer()
    {
        yield return new WaitForSeconds(explodeTime);
        waterCollider.enabled = true;
        _animator.SetTrigger(POPUPTHEWATER);
        SoundManager.PlaySound(ClipName.WaterSpellExplosion,transform.position);
        yield return new WaitForSeconds(disableCollidedTime);
        waterCollider.enabled = false;
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
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
            
            if (other.TryGetComponent(out FirePower otherFireCharge))
            {
                float df = otherFireCharge.GetDecayingFireFactor();
                otherFireCharge.DecreaseDecayingFactor(df - 0.20f);
            }
        }
    }
}
