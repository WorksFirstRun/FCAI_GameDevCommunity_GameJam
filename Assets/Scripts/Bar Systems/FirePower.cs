using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FirePower : MonoBehaviour , IDecaySwitch
{
    [SerializeField] private Light2D fireLightSource;
    [SerializeField] private float pivot;
    [SerializeField] private float totalLightAmount;
    [SerializeField] private float maxFullChargeTime;
    [SerializeField] private UIBars fireChargeUI;
    private float decayingFactor;
    private float fullChargeTime;
    private DecayState currentChargeState;
    
    public enum  DecayState
    {
        fullCharge,
        Decaying,
    }

    public float GetDecayingFireFactor()
    {
        return decayingFactor;
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
                    SwitchLightState(DecayState.Decaying);
                }
                break;
            case DecayState.Decaying:
                if (decayingFactor >= 0)
                {
                    decayingFactor -= 0.00085f;
                    AdjustLight(pivot + (totalLightAmount * decayingFactor));
                }

                break;
        }
        
    }

    private void AdjustLight(float newValue)
    {
        fireChargeUI.AdjustTheFireBar(decayingFactor);
        fireLightSource.pointLightInnerRadius = newValue;
        fireLightSource.pointLightOuterRadius = newValue + 0.5f;
    }

    public void SwitchLightState(DecayState state)
    {
        currentChargeState = state;
        fullChargeTime = maxFullChargeTime;
    }

    public void IncreaseDecayingFactor(float df)
    {
        df = Math.Clamp(df, 0, 1);
        df = Math.Max(decayingFactor, df);
        decayingFactor = df;
        AdjustLight(pivot + (totalLightAmount * df));
    }

    public void DecreaseDecayingFactor(float df)
    {
        df = Math.Clamp(df, 0, 1);
        decayingFactor = df;
        AdjustLight(pivot + (totalLightAmount * df));
    }

    /*
    public void AdjustFireBar()
    {
        fireChargeUI.AdjustTheFireBar(decayingFactor);
    }
    */
    
    
}
