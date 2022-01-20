using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public abstract class AIGeneral : MonoBehaviour
{
    [SerializeField] protected BehaviorTree behaviorTree;

    [SerializeField] protected NavMeshAgent agent;

    [SerializeField] protected float executionInterval;

    [SerializeField] protected Transform defesePoint;

    [SerializeField] protected CombatOrder combatOrder=CombatOrder.DEFEND;

    [SerializeField] protected Collider objCollider;

    public Action OnAction;

    protected bool alreadyConstructBehaviorTree=false;

    public abstract void  StartBehaviorTree();

    public abstract void StopBehaviorTree();

    public CombatOrder GetCombatOrder()
    {
        return combatOrder;
    }    

    public void SetCombatOrder(CombatOrder order)
    {
        combatOrder = order;
    }

    public Transform GetDefesePoint()
    {
        return defesePoint;
    }

    public void EnableCollider(bool enabled)
    {
        objCollider.enabled = enabled;
    }

    protected virtual void OnEnable()
    {
        if (agent.enabled == false)
            StartCoroutine(EnableNavMesh());
    }

    IEnumerator EnableNavMesh()
    {
        yield return null;
        agent.enabled = true;
        
    }
}

public interface IDefensePoint
{
    void SetDefensePoint(Transform point);
}

public interface IGenericMethod
{
    void Action();
}

public interface IHealer
{
    bool GetCanHeal();
}


public interface IMarkTarget
{
    void MarkTarget(Transform reference);

    Transform GetMarkedTarget();
}


public enum CombatOrder
{
    RETREAT,
    DEFEND,
    ATACK
}
