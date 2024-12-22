using System;



public class BossHealth : EnemyHealth
{
    public event Action OnBossHealthChanged;
    private float nextHealthPointsTriger;

    private void Start()
    {
        nextHealthPointsTriger = currentHealth - 20;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth <= nextHealthPointsTriger && !died)
        {
            OnBossHealthChanged?.Invoke();
            DropSystem.Instance.DropItem(enemyLoot, transform.position);
            nextHealthPointsTriger -= 20;
        }
    }
    
    
    
}
