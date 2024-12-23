using System;
using System.Collections;
using UnityEngine;

public  class BaseHealthScript : MonoBehaviour
{
    [SerializeField] protected float maxHealth;
    protected float currentHealth;
    public event EventHandler<CurrentHealthArgs> onHealthUpdateBar;
    protected bool died;

    public class CurrentHealthArgs : EventArgs
    {
        public float currentHealth;
    }

    public void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthUpdateBar?.Invoke(this , new CurrentHealthArgs()
        {
            currentHealth = currentHealth/maxHealth
        });
        // maybe Invoke Hit Animation here
        
        if (currentHealth <= 0 && !died)
        {
            died = true;
        }
    }

    private IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(1);
       
        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        onHealthUpdateBar?.Invoke(this , new CurrentHealthArgs()
        {
            currentHealth = currentHealth/maxHealth
        });
    }


    public void SetMaxHealth(float amount) // maybe will need it in the xp system
    {
        
    }

    private  void OnDestroy()
    {
        onHealthUpdateBar = null;
    }
}