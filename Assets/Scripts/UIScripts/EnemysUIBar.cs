using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemysUIBar : MonoBehaviour
{
    [SerializeField] private Image hp;
    [SerializeField] private BaseHealthScript _baseHealthScript;
    
    private void Start()
    {
        _baseHealthScript.onHealthUpdateBar += OnHealthBarUpdate;
    }

    private void OnHealthBarUpdate(object sender, BaseHealthScript.CurrentHealthArgs e)
    {
        hp.fillAmount = e.currentHealth;
    }
}
