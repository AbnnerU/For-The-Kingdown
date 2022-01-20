using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OnNewOrder: UnityEvent { }

public class UnitysManager : MonoBehaviour
{
    [SerializeField] private CombatOrder generalCombatOrder=CombatOrder.DEFEND;
    [SerializeField] private Positions positions;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<Unitys> unitys;

    [SerializeField] private List<PointStatus> pointStatus = new List<PointStatus>();

    public OnNewOrder OnRetreat;
    public OnNewOrder OnDefend;   
    public OnNewOrder OnAtack;

    public Action<GameObject> OnRemoveObj;

    public Action<int> OnAddObj;

    private Transform[] points;

    private int currentvalue = 0;
    private int maxPoints;
    private int count;

    private void Awake()
    {
         points = positions.GetPoints();

        foreach (Transform t in points)
        {
            PointStatus newPointStatus = new PointStatus();
            newPointStatus.AI = null;
            newPointStatus.occupiedBy = null;
            newPointStatus.position = t;
            newPointStatus.unityType = UnityType.EMPTY;

            pointStatus.Add(newPointStatus);
        }

       
        maxPoints = pointStatus.Count;

        count = unitys.Count;
    }

    public bool SpawnUnity(GameObject prefabRef,int goldAmount, out int cost)
    {
        int index = unitys.FindIndex(0, x => x.prefab == prefabRef);

        if (currentvalue < maxPoints)
        {
            if (index >= 0)
            {
                if (goldAmount < unitys[index].cost)
                {
                    cost = 0;
                    return false;
                }

                AIGeneral aIGeneral = unitys[index].prefab.GetComponent<AIGeneral>();
                IDefensePoint defensePoint = unitys[index].prefab.GetComponent<IDefensePoint>();
                            

                if (aIGeneral != null && defensePoint != null)
                {
                   
                    GameObject obj = PoolManager.SpawnObject(unitys[index].prefab, spawnPoint.position, Quaternion.identity);

                    AIGeneral AI = obj.GetComponent<AIGeneral>();
                    IDefensePoint defense = obj.GetComponent<IDefensePoint>();
                    HealthBasics healthManager = obj.GetComponent<HealthBasics>();

                    AI.SetCombatOrder(generalCombatOrder);

                    AI.EnableCollider(true);

                    IAtackAI atackAI = obj.GetComponent<IAtackAI>();

                    if (atackAI == null)
                    {
                        atackAI = obj.GetComponentInChildren<IAtackAI>();
                    }


                    if (atackAI != null)
                    {
                        atackAI.CanAtack();
                    }

                    ChoosePosition(AI,defense, unitys[index].unityType);

                    AI.StartBehaviorTree();

                    currentvalue++;

                    OnAddObj?.Invoke(currentvalue);

                    healthManager.SetHealthToMax();

                    if (healthManager.OnDeath != null)
                    {
                        int invocationLenght = healthManager.OnDeath.GetInvocationList().Length;
                        for (int i = 0; i < invocationLenght; i++)
                        {
                            if (healthManager.OnDeath.GetInvocationList()[i].Method.Name == "RemoveUnity")
                            {
                                cost = unitys[index].cost;
                                return true;
                            }
                        }                     
                    }
                    healthManager.OnDeath += RemoveUnity;
                    cost = unitys[index].cost;
                    return true;
                }
                else
                {                  
                    print("Prefab don't have AIGeneral or IDefensepoint");
                    cost = 0;
                    return false;
                }
            }
            else
            {
                print("Prefab do not exist");
                cost = 0;
                return false;
            }
        }
        cost = 0;
        return false;
    }

    public bool SpawnUnity(GameObject prefabRef, int goldAmount, out int cost, out GameObject objSpanwed)
    {
        int index = unitys.FindIndex(0, x => x.prefab == prefabRef);

        if (currentvalue < maxPoints)
        {
            if (index >= 0)
            {
                if (goldAmount < unitys[index].cost)
                {
                    objSpanwed = null;
                    cost = 0;
                    return false;
                }

                AIGeneral aIGeneral = unitys[index].prefab.GetComponent<AIGeneral>();
                IDefensePoint defensePoint = unitys[index].prefab.GetComponent<IDefensePoint>();


                if (aIGeneral != null && defensePoint != null)
                {

                    GameObject obj = PoolManager.SpawnObject(unitys[index].prefab, spawnPoint.position, Quaternion.identity);

                    AIGeneral AI = obj.GetComponent<AIGeneral>();
                    IDefensePoint defense = obj.GetComponent<IDefensePoint>();
                    HealthBasics healthManager = obj.GetComponent<HealthBasics>();

                    AI.SetCombatOrder(generalCombatOrder);

                    AI.EnableCollider(true);

                    IAtackAI atackAI = obj.GetComponent<IAtackAI>();

                    if (atackAI == null)
                    {
                        atackAI = obj.GetComponentInChildren<IAtackAI>();
                    }


                    if (atackAI != null)
                    {
                        atackAI.CanAtack();
                    }

                    ChoosePosition(AI, defense, unitys[index].unityType);

                    AI.StartBehaviorTree();

                    currentvalue++;

                    OnAddObj?.Invoke(currentvalue);

                    healthManager.SetHealthToMax();

                    if (healthManager.OnDeath != null)
                    {
                        int invocationLenght = healthManager.OnDeath.GetInvocationList().Length;
                        for (int i = 0; i < invocationLenght; i++)
                        {
                            if (healthManager.OnDeath.GetInvocationList()[i].Method.Name == "RemoveUnity")
                            {
                                objSpanwed = obj;
                                cost = unitys[index].cost;
                                return true;
                            }
                        }
                    }
                    healthManager.OnDeath += RemoveUnity;

                    objSpanwed = obj;
                    cost = unitys[index].cost;
                    return true;
                }
                else
                {
                    print("Prefab don't have AIGeneral or IDefensepoint");
                    objSpanwed = null;
                    cost = 0;
                    return false;
                }
            }
            else
            {
                print("Prefab do not exist");
                objSpanwed = null;
                cost = 0;
                return false;
            }
        }
        objSpanwed = null;
        cost = 0;
        return false;
    }

    public void SpawnUnity(GameObject prefabRef)
    {
        int index = unitys.FindIndex(0, x => x.prefab == prefabRef);

        if (currentvalue < maxPoints)
        {
            if (index >= 0)
            {

                AIGeneral aIGeneral = unitys[index].prefab.GetComponent<AIGeneral>();
                IDefensePoint defensePoint = unitys[index].prefab.GetComponent<IDefensePoint>();


                if (aIGeneral != null && defensePoint != null)
                {
                   

                    GameObject obj = PoolManager.SpawnObject(unitys[index].prefab, spawnPoint.position, Quaternion.identity);

                    AIGeneral AI = obj.GetComponent<AIGeneral>();
                    IDefensePoint defense = obj.GetComponent<IDefensePoint>();
                    HealthBasics healthManager = obj.GetComponent<HealthBasics>();

                    AI.SetCombatOrder(generalCombatOrder);

                    IAtackAI atackAI = obj.GetComponent<IAtackAI>();

                    if (atackAI == null)
                    {
                        atackAI = obj.GetComponentInChildren<IAtackAI>();
                    }


                    if (atackAI != null)
                    {
                        atackAI.CanAtack();
                    }

                    ChoosePosition(AI, defense, unitys[index].unityType);

                    AI.StartBehaviorTree();

                    currentvalue++;

                    OnAddObj?.Invoke(currentvalue);

                    healthManager.SetHealthToMax();

                    if (healthManager.OnDeath != null)
                    {
                        int invocationLenght = healthManager.OnDeath.GetInvocationList().Length;
                        for (int i = 0; i < invocationLenght; i++)
                        {
                            if (healthManager.OnDeath.GetInvocationList()[i].Method.Name == "RemoveUnity")
                            {
                                return;
                            }
                        }
                    }
                    healthManager.OnDeath += RemoveUnity;

                }
                else
                {
                    print("Prefab don't have AIGeneral or IDefensepoint");
                }
            }
            else
            {
                print("Prefab do not exist");
            }
        }

    }

    public void ChoosePosition(AIGeneral aIGeneral, IDefensePoint defensePoint, UnityType unityType)
    {
        int id = 0;
        foreach(PointStatus ps in pointStatus)
        {
            if(ps.unityType == UnityType.EMPTY)
            {
                defensePoint.SetDefensePoint(ps.position);

                ps.AI = aIGeneral;

                ps.occupiedBy = defensePoint;

                ps.unityType = unityType;
                return;
            }

            if (unityType == UnityType.MELEE && ps.unityType != UnityType.MELEE)
            {
                Reposition(id,aIGeneral, defensePoint,unityType,ps);

                return ;

            }
            else if (unityType == UnityType.RANGED && ps.unityType == UnityType.HEALER)
            {
                Reposition(id,aIGeneral,defensePoint, unityType, ps);

                return;
            }

            id++;
        }
    }

    public void RemoveUnity(GameObject reference)
    {
        IDefensePoint defensePoint = reference.GetComponent<IDefensePoint>();

        if (defensePoint!=null)
        {
            int index = pointStatus.FindIndex(0, x => x.occupiedBy == defensePoint);

            if (index >= 0)
            {
                
                if (index + 1 <= maxPoints)
                {
                    OnRemoveObj?.Invoke(reference);
                    List<PointStatus> temp = new List<PointStatus>();
                    for (int i = index + 1; i < pointStatus.Count; i++)
                    {                      
                        PointStatus newPointStatus = new PointStatus();
                        newPointStatus.AI = pointStatus[i].AI;
                        newPointStatus.occupiedBy = pointStatus[i].occupiedBy;
                        newPointStatus.position = pointStatus[i].position;
                        newPointStatus.unityType = pointStatus[i].unityType;
                        temp.Add(newPointStatus);
                    }

                    pointStatus[index].AI = null;
                    pointStatus[index].occupiedBy = null;
                    pointStatus[index].unityType = UnityType.EMPTY;

                    int updateIndex = 0;
                    int updateCount = temp.Count;
                    //Update
                    for (int i=index;i < pointStatus.Count; i++)
                    {
                        if (updateIndex < updateCount)
                        {
                            pointStatus[i].AI = temp[updateIndex].AI;
                            pointStatus[i].occupiedBy = temp[updateIndex].occupiedBy;
                            pointStatus[i].unityType = temp[updateIndex].unityType;

                            if (pointStatus[i].occupiedBy != null)
                                pointStatus[i].occupiedBy.SetDefensePoint(pointStatus[i].position);
                        }
                        else
                        {
                            pointStatus[i].AI = null;
                            pointStatus[i].occupiedBy = null;
                            pointStatus[i].unityType = UnityType.EMPTY;
                           
                        }

                        updateIndex++;
                    }

                    
                    currentvalue--;
                                    
                }
            }
            else
            {
                print("Index not finded: " + reference.name);
            }
        }
        else
        {
            print("Reference don't have IDefensePoint :" + reference.name);
        }
    }

    public void Reposition(int id, AIGeneral aIGeneral, IDefensePoint defensePoint, UnityType unityType, PointStatus ps)
    {
        if (id + 1 <= maxPoints)
        {
            List<PointStatus> temp = new List<PointStatus>();
            for (int i = id; i < pointStatus.Count; i++)
            {
                PointStatus newPointStatus = new PointStatus();
                newPointStatus.AI = pointStatus[i].AI;
                newPointStatus.occupiedBy = pointStatus[i].occupiedBy;
                newPointStatus.position = pointStatus[i].position;
                newPointStatus.unityType = pointStatus[i].unityType;
                temp.Add(newPointStatus);
            }

            defensePoint.SetDefensePoint(ps.position);

            ps.AI = aIGeneral;

            ps.occupiedBy = defensePoint;

            ps.unityType = unityType;

            int updateIndex = 0;
            //Update
            for (int i = id + 1; i < pointStatus.Count; i++)
            {
                pointStatus[i].AI = temp[updateIndex].AI;
                pointStatus[i].occupiedBy = temp[updateIndex].occupiedBy;
                pointStatus[i].unityType = temp[updateIndex].unityType;

                if (pointStatus[i].occupiedBy != null)
                    pointStatus[i].occupiedBy.SetDefensePoint(pointStatus[i].position);

                updateIndex++;
            }


        }
        else
        {
            print("id out of range: " + id);
        }

    }

    public void SetNewOrder(int id)
    {      
        CombatOrder newOrder=CombatOrder.DEFEND;

        if (id == 0)
            newOrder = CombatOrder.DEFEND;
        else if (id == 1)
            newOrder = CombatOrder.ATACK;
        else
            newOrder = CombatOrder.RETREAT;

        CallOrderEvent(newOrder);

        generalCombatOrder = newOrder;

        foreach(PointStatus ps in pointStatus)
        {
            if(ps.AI!=null)
            ps.AI.SetCombatOrder(newOrder);
        }
    }

    public void SetNewOrder(CombatOrder newOrder)
    {
        CallOrderEvent(newOrder);

        generalCombatOrder = newOrder;

        foreach (PointStatus ps in pointStatus)
        {
            if (ps.AI != null)
                ps.AI.SetCombatOrder(newOrder);
        }
    }

    public void CallOrderEvent(CombatOrder orderRef)
    {
        if (orderRef == CombatOrder.RETREAT)
        {
            OnRetreat?.Invoke();
        }
        else if(orderRef == CombatOrder.DEFEND)
        {
            OnDefend?.Invoke();
        }
        else
        {
            OnAtack?.Invoke();
        }
    }

    public CombatOrder GetOrder()
    {
        return generalCombatOrder;
    }

    public int GetCurrentUnitysValue()
    {
        return currentvalue;
    }

    public List<Unitys> GetUnitys()
    {
        return unitys;
    }

    public void TryStopUnitys()
    {
        foreach (PointStatus ps in pointStatus)
        {
            if (ps.AI != null)
                ps.AI.StopBehaviorTree();
        }
    }
}

[System.Serializable]
public class PointStatus
{
    public Transform position;

    public AIGeneral AI;

    public IDefensePoint occupiedBy;

    public UnityType unityType;
}


[System.Serializable]
public struct Unitys
{
    public GameObject prefab;
    public UnityType unityType;
    public int cost;
}


public enum UnityType
{
    EMPTY,
    MELEE,
    RANGED,
    HEALER
}