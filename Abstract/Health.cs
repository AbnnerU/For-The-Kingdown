using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HealthBasics : HealthEvents, IDamageble, IHealable
{
    [SerializeField] protected int maxHealth;

    [SerializeField] protected GameObject objRef;

    public OnSpawnEvent onSpawnEvent;

    public OnHealEvent OnHealEvent;

    public OnDeathCustomEvent OnDeathEvent;


    protected int currentHealth;

    protected bool alive = true;

    public abstract void Heal(int value);

    public abstract void OnHit(int damageValue);

    public virtual void SetHealthToMax()
    {
        onSpawnEvent?.Invoke();
        currentHealth = maxHealth;
        OnSpawned?.Invoke();
        alive = true;
    }

    public virtual int GetCurrentHealth()
    {
        return currentHealth;
    }

    public virtual int GetMaxHealth()
    {   
        return maxHealth;
    }

    public bool IsAlive()
    {
        return alive;
    }
}

[System.Serializable]
public class OnDeathCustomEvent : UnityEvent { }

[System.Serializable]
public class OnSpawnEvent : UnityEvent { }

[System.Serializable]
public class OnHealEvent : UnityEvent { }

public abstract class HealthEvents : MonoBehaviour
{
    public Action OnHitted;

    public Action OnHeal;

    public Action OnSpawned;

    public Action<GameObject> OnDeath;
}

public interface IDamageble
{
    void OnHit(int damageValue);
}

public interface IHealable
{
    void Heal(int value);
}