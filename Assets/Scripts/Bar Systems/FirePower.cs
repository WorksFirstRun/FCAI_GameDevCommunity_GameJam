using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirePower : MonoBehaviour , IDecaySwitch
{
    [SerializeField] private Light2D fireLightSource;
    [SerializeField] private float pivot;
    [SerializeField] private float totalLightAmount;
    [SerializeField] private float maxFullChargeTime;
    private float decayingFactor;
    private float fullChargeTime;
    private DecayState currentChargeState;
    
    public enum  DecayState
    {
        fullCharge,
        Decaying,
    }

    private void Start()
    {
        decayingFactor = 1;
        fullChargeTime = maxFullChargeTime;
        currentChargeState = DecayState.fullCharge;
        AdjustLight(totalLightAmount + pivot);
    }

    private void FixedUpdate()
    {
        switch (currentChargeState)
        {
            case DecayState.fullCharge:
                fullChargeTime -= Time.deltaTime;
                if (fullChargeTime < 0)
                {
                    SwitchLightState(DecayState.Decaying,decayingFactor);
                }
                break;
            case DecayState.Decaying:
                if (decayingFactor > 0)
                {
                    decayingFactor -= 0.00085f;
                    AdjustLight(pivot + (totalLightAmount * decayingFactor));
                }

                break;
        }
        
    }

    public void AdjustLight(float newValue)
    {
        fireLightSource.pointLightInnerRadius = newValue;
        fireLightSource.pointLightOuterRadius = newValue + 0.5f;
    }

    public void SwitchLightState(DecayState state,float df)
    {
        df = Math.Clamp(df, 0, 1);
        df = Math.Max(decayingFactor, df);
        if (state == DecayState.fullCharge)
        {
            AdjustLight(pivot + (totalLightAmount * df)); // set it to full again
        }
        currentChargeState = state;
        // reset timers 
        fullChargeTime = maxFullChargeTime;
        decayingFactor = df;
    }
    
    
}
