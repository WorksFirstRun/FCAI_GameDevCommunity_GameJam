

using System;
using System.Collections;
using UnityEngine;

public class PlayerAnimation : AnimationAndVisualsScript<PlayerAnimations>
{
   
   public override Enum GetDeathAnimationEnum()
   {
      return PlayerAnimations.Death;
   }

   // below code just copied from chat-gpt because LESS THAN 1 DAY AND I NEED TO FINISH FAST
   
   [SerializeField] private PlayerHealth _playerHealth;
   private SpriteRenderer playerRenderer;
   private Color originalColor;
   private bool isDamaged = false;
   private float flashDuration = 0.5f;
   private float flashIntensity = 1.5f; 

   private void Start()
   {
      playerRenderer = GetComponent<SpriteRenderer>();
      originalColor = playerRenderer.material.color;
      _playerHealth.OnPlayerDamaged += OnDamaged;
   }

   private void OnDamaged() 
   {
      if (!isDamaged)
      {
         StartCoroutine(DamageFlashEffect());
      }
   }

   private IEnumerator DamageFlashEffect()
   {
      isDamaged = true;

      float elapsedTime = 0f;
      while (elapsedTime < flashDuration)
      {
         // Lerp between the original color and a red tint for intensity
         playerRenderer.material.color = Color.Lerp(originalColor, Color.red * flashIntensity, Mathf.PingPong(Time.time * 2f, 1));
         elapsedTime += Time.deltaTime;
         yield return null;
      }

      // Return to the original color after flashing
      playerRenderer.material.color = originalColor;
      isDamaged = false;
   }
}
