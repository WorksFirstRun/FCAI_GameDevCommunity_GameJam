using System;
using UnityEngine;

public class PlayerHealth : BaseHealthScript
{
    private const string DECLARELOSE = "DeclareLose";
    [SerializeField] private Player _playerLogic;
    [SerializeField] private BaseAnimationAndVisualsScript playerAnimation;
    private bool isTriggered;
    private float deathTime;
    public event Action OnPlayerDamaged;

    private void Start()
    {
        deathTime = playerAnimation.GetAnimationClipTime(playerAnimation.GetDeathAnimationEnum());
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (died && !isTriggered)
        {
            isTriggered = true;
            _playerLogic.DisablePlayerBehaviour();
            playerAnimation.SwitchAnimation(PlayerAnimations.Death);
            Invoke(DECLARELOSE,deathTime);
        }
        else
        {
            OnPlayerDamaged?.Invoke();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent<IHealable>(out IHealable potion)) return;
        if (potion.flag) return; // bug fix where the player take heal twice instead of once
        
        potion.flag = true;
        potion.HealEntity(this);
        Destroy(other.gameObject);
    }

    private void DeclareLose()
    {
        GameManager.Instance.DeclarePlayerLost();
    }
    
}
