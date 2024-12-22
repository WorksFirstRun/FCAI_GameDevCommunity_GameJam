

using System;

public class PlayerAnimation : AnimationAndVisualsScript<PlayerAnimations>
{
   public override Enum GetDeathAnimationEnum()
   {
      return PlayerAnimations.Death;
   }
}
