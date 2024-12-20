using System;


public class WaterWitchAnimationsVisuals : AnimationAndVisualsScript<WaterWitchAnimation>
{ 
        public override Enum GetDeathAnimationEnum() 
        { 
                return WaterWitchAnimation.Death;
        }
}
