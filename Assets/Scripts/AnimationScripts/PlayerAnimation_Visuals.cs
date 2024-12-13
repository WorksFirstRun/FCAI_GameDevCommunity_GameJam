using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation_Visuals : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform playerVisualT;
    private Dictionary<string, float> animationsTiming;
    private Animations currentAnimation;

    private void Start()
    {
        animationsTiming = new Dictionary<string, float>();
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        
        foreach (AnimationClip clip in controller.animationClips)
        {
           animationsTiming.Add(clip.name,clip.length);
        }
    }

    public enum Animations
    {
        Idle,
        Run,
        Attack1,
        Attack2,
        Attack3,
        Dash,
        Hit,
        Death,
        ChargeSpell,
        CastSpell
    }

    public void SwitchAnimation(Animations newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        currentAnimation = newAnimation;
        _animator.Play(currentAnimation.ToString());
    }

    public void AdjustVisualDirection(float xDirection)
    {
        if (xDirection < 0)
        {
            Quaternion newRotation =  Quaternion.Euler(0,180,0);
            playerVisualT.rotation = newRotation;
        }
        else if (xDirection > 0)
        {
            Quaternion newRotation =  Quaternion.Euler(0,0,0);
            playerVisualT.rotation = newRotation;
        }
    }


    public float GetAnimationClipTime(Animations animationName)
    {
        return animationsTiming[animationName.ToString()];
    }
}
