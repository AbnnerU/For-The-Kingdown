using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMoveToPoint : BTnode
{
    private NavMeshAgent agent;

    private AIGeneral AI;

    private Transform pointTransform;

    private float distance;

    public BTMoveToPoint(AIGeneral AI, NavMeshAgent agent, Transform pointTransform)
    {
        this.AI = AI;

        this.agent = agent;

        this.pointTransform = pointTransform;

        distance = agent.stoppingDistance;
    }

    public BTMoveToPoint(AIGeneral AI, NavMeshAgent agent, Transform pointTransform,float distance)
    {
        this.AI = AI;

        this.agent = agent;

        this.pointTransform = pointTransform;

        this.distance = distance;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (agent.enabled == false)
        {
            status = BTstatus.FAILURE;
            yield break;
        }

        agent.isStopped = false;

        agent.SetDestination(pointTransform.position);

        while (agent != null && agent.enabled==true)
        {

            if (agent.pathPending == false && agent.remainingDistance < distance)
            {

                agent.isStopped = true;
                status = BTstatus.SUCCESS;

                yield break;
            }

            yield return null;
        }

        status = BTstatus.FAILURE;

        yield break;
    }
}
