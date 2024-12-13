
using System;
using UnityEngine;

public class FireCharge : MonoBehaviour
{
    [SerializeField]
    private float decayFactor;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDecaySwitch>(out IDecaySwitch obj))
        {
            obj.SwitchLightState(FirePower.DecayState.fullCharge);
            obj.IncreaseDecayingFactor(decayFactor);
            Destroy(gameObject);
        }
    }
}
