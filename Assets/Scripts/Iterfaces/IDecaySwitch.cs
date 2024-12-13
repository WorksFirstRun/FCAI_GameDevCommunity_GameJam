
using UnityEngine;

public interface IDecaySwitch 
{
    public void SwitchLightState(FirePower.DecayState state);
    public void IncreaseDecayingFactor(float df);
}
