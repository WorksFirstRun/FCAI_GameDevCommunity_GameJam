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
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private BaseHealthScript _baseHealthScript;
    
    private void Awake()
    {
        firePowerText.text = "100";
        chargeBar.fillAmount = 0;
        HideChargeBar();
        _baseHealthScript.onHealthUpdateBar += OnHealthBardUpdated;
    }

    private void OnHealthBardUpdated(object sender, BaseHealthScript.CurrentHealthArgs e)
    {
        hp.fillAmount = e.currentHealth;
        hpText.text = ((e.currentHealth * 100f)).ToString();
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
