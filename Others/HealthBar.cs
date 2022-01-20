using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthManager healthManager;

    [SerializeField] private Slider slider;

    private void Awake()
    {
        if (healthManager)
        {
            healthManager.OnHitted += HealthChanged;
            healthManager.OnHeal += HealthChanged;
            healthManager.OnSpawned += HealthChanged;
        }

        if (slider)
        {

            slider.maxValue = healthManager.GetMaxHealth();
            slider.value = healthManager.GetMaxHealth();
        }
    }

    private void HealthChanged()
    {
        slider.value = healthManager.GetCurrentHealth();
    }
}
