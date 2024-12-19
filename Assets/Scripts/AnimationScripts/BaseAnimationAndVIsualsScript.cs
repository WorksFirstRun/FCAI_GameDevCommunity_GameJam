using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAndVisualsScript<TAnimation> : MonoBehaviour where TAnimation : Enum
{
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform playerVisualT;
    private Dictionary<string, float> animationsTiming;
    private TAnimation currentAnimation;

    private void Start()
    {
        animationsTiming = new Dictionary<string, float>();
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        
        foreach (AnimationClip clip in controller.animationClips)
        {
            animationsTiming.Add(clip.name, clip.length);
        }
    }

    public  void SwitchAnimation(TAnimation newAnimation)
    {
        if (currentAnimation.Equals(newAnimation)) return;

        currentAnimation = newAnimation;
        _animator.Play(currentAnimation.ToString(),0);
    }

    public void AdjustVisualDirection(float xDirection)
    {
        if (xDirection < 0)
        {
            Quaternion newRotation = Quaternion.Euler(0, 180, 0);
            playerVisualT.rotation = newRotation;
        }
        else if (xDirection > 0)
        {
            Quaternion newRotation = Quaternion.Euler(0, 0, 0);
            playerVisualT.rotation = newRotation;
        }
    }

    public float GetAnimationClipTime(TAnimation animationName)
    {
        return animationsTiming[animationName.ToString()];
    }
}