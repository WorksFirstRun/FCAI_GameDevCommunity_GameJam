using System;

public class DarkSamuraiAnimationsVisuals : AnimationAndVisualsScript<DarkSamuraiAnimation>
{
   public override Enum GetDeathAnimationEnum()
   {
      return DarkSamuraiAnimation.Death;
   }
}
