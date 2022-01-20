using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthManager : HealthBasics
{
   
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public override void OnHit(int damageValue)
    {
        if (alive)
        {
            currentHealth -= damageValue;

            OnHitted?.Invoke();
            if (currentHealth <= 0)
            {
                alive = false;
                if (objRef)
                {
                    OnDeath?.Invoke(objRef);
                }
                else
                {
                    OnDeath?.Invoke(gameObject);
                }

                OnDeathEvent?.Invoke();             
            }
        }
    }

    public void ReleaseObject()
    {
        PoolManager.ReleaseObject(gameObject);
    }

    public override void Heal(int value)
    {
        currentHealth += value;

        OnHeal?.Invoke();

        OnHealEvent?.Invoke();

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
