using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBars : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private Image fireCharge;
    [SerializeField] private Image chargeBar;
    [SerializeField] private Transform chargeBarParent;
    [SerializeField] private TextMeshProUGUI firePowerText;

    private void Awake()
    {
        firePowerText.text = "100";
        chargeBar.fillAmount = 0;
        HideChargeBar();
    }

    public void AdjustTheFireBar(float amount)
    {
        fireCharge.fillAmount = amount;
        firePowerText.text = ((int) (amount * 100)).ToString();
    }

    public void ShowChargeBar()
    {
        chargeBarParent.gameObject.SetActive(true);
    }

    public void HideChargeBar()
    {
        chargeBarParent.gameObject.SetActive(false);
    }

    public void FillChargeBar(float amount)
    {
        amount = Math.Clamp(amount, 0, 1);
        chargeBar.fillAmount = amount;
    }
    
}
