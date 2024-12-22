using UnityEngine;

public class HealthPotion : MonoBehaviour, IHealable
{
    [SerializeField] private float healAmount;
    public bool flag { get; set; }

   

    public void HealEntity(BaseHealthScript health)
    {
        health.SetCurrentHealth(healAmount);
    }
    
}