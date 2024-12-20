using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAnimationAndVisualsScript : MonoBehaviour
{
    public abstract void SwitchAnimation(Enum newAnimation);
    public abstract float GetAnimationClipTime(Enum animationName);
    public abstract void AdjustVisualDirection(float xDirection);

    public abstract float[] GetAnimationFrames(Enum animationName);

    public virtual Enum GetDeathAnimationEnum()
    {
        Debug.LogError("Abstract Death Animation Getter Called");
        return null;
    }
}

public class AnimationAndVisualsScript<TAnimation> : BaseAnimationAndVisualsScript where TAnimation : Enum
{
    [System.Serializable]
    public class AnimationFrameData
    {
        public float[] frames;
    }
    
    [SerializeField] protected Animator _animator;
    [SerializeField] protected Transform playerVisualT;
    [SerializeField] protected List<AnimationFrameData> animationsFrames;
    [SerializeField] protected TAnimation[] animationsFramesName;
    private Dictionary<string, float> animationsTiming;
    private Dictionary<string, float[]> animationSpecificFrames;
    private TAnimation currentAnimation;
    
    
    private void Awake()
    {
        animationsTiming = new Dictionary<string, float>();
        animationSpecificFrames = new Dictionary<string, float[]>(); // Initialize here
        RuntimeAnimatorController controller = _animator.runtimeAnimatorController;
        
        foreach (AnimationClip clip in controller.animationClips)
        {
            animationsTiming.Add(clip.name, clip.length);
        }

        for (int i = 0; i < animationsFramesName.Length; i++)
        {
            animationSpecificFrames.Add(animationsFramesName[i].ToString(),animationsFrames[i].frames);
        }
        
    }

     public override void SwitchAnimation(Enum newAnimation)
    {
        if (!(newAnimation is TAnimation)) return;

        TAnimation animation = (TAnimation) newAnimation;
        if (currentAnimation.Equals(animation)) return;

        currentAnimation = animation;
        _animator.Play(currentAnimation.ToString(), 0);
    }

    public override float GetAnimationClipTime(Enum animationName)
    {
        return animationsTiming[animationName.ToString()];
    }
    
    public override void AdjustVisualDirection(float xDirection)
    {
        if (xDirection < 0)
        {
            playerVisualT.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (xDirection > 0)
        {
            playerVisualT.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override float[] GetAnimationFrames(Enum animationName)
    {
        string animationKey = animationName.ToString();

        if (animationSpecificFrames.ContainsKey(animationKey))
        {
            return animationSpecificFrames[animationKey];
        }

        Debug.LogError($"Animation frames for {animationKey} not found.");
        return Array.Empty<float>();
    }
    
}

public enum DarkSamuraiAnimation
{
    Idle,
    Run,
    Hit,
    Death,
    Attack,
}

public enum WaterWitchAnimation
{
    Run,
    Idle,
    Hit,
    Death,
    ChargeSpell,
    WaterGroundAttack
}

public enum PlayerAnimations
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
    CastSpell,
    SlashEffect
}
