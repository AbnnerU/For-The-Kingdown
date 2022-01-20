using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTIsOnDefensePoint : BTnode
{
    private NavMeshAgent agent;

    private AIGeneral AI;

    private Transform _tranform;

    private float distance;

    public BTIsOnDefensePoint(AIGeneral AI, NavMeshAgent agent, float distance)
    {
        this.AI = AI;
        this.agent = agent;

        _tranform = agent.transform;

        this.distance = distance;
    }


    public BTIsOnDefensePoint(AIGeneral AI, NavMeshAgent agent)
    {
        this.AI = AI;
        this.agent = agent;

        _tranform = agent.transform;

        distance = agent.stoppingDistance;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if((AI.GetDefesePoint().position - _tranform.position).magnitude <= distance)
        {
            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }

        //Debug.Log("Está no ponto: " + status.ToString());
        yield break;
    }
}
