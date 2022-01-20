using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTGoToDefensePoint : BTnode
{
    private NavMeshAgent agent;

    private AIGeneral AI;

    private Transform _tranform;

    public BTGoToDefensePoint(AIGeneral AI, NavMeshAgent agent)
    {
        this.AI = AI;
        this.agent = agent;

        _tranform = agent.transform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (agent.enabled == false)
        {
            status = BTstatus.FAILURE;

            yield break;
        }

        float distance = agent.stoppingDistance;

        agent.isStopped = false;

        agent.SetDestination(AI.GetDefesePoint().position);

        while (agent != null && agent.enabled==true)
        {
            if (agent.pathPending == false && agent.remainingDistance < distance)
            {
                
                status = BTstatus.SUCCESS;

                yield break;
            }

            yield return null;
        }

        status = BTstatus.FAILURE;

        yield break;
    }
}
