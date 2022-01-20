using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTMoveToEnemy : BTnode
{
    private IMarkTarget aICombat;

    private NavMeshAgent agent;

    private float updateInterval;

    private float distance;

    private Transform _transform;

    public BTMoveToEnemy(IMarkTarget aICombat, NavMeshAgent agent, float updateInterval)
    {
        this.aICombat = aICombat;
        this.agent = agent;
        this.updateInterval = updateInterval;

        distance = agent.stoppingDistance;

        _transform = agent.transform;
    }

    public BTMoveToEnemy(IMarkTarget aICombat, NavMeshAgent agent, float updateInterval, float distance)
    {
        this.aICombat = aICombat;
        this.agent = agent;
        this.updateInterval = updateInterval;

        this.distance = distance;

        _transform = agent.transform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        Transform currentTarget = aICombat.GetMarkedTarget();

        agent.isStopped = true;

        if (Vector3.Distance(_transform.position,currentTarget.position) <= distance)
        {
            Debug.Log((currentTarget.position - _transform.position).magnitude);
            agent.isStopped = true;

            status = BTstatus.SUCCESS;

            yield break;
        }      


        while (aICombat.GetMarkedTarget() == currentTarget && currentTarget.gameObject.activeSelf == true)
        {        
            agent.SetDestination(currentTarget.position);

            yield return null;

            if (agent.pathPending == false)
                agent.isStopped = false;

            if (agent.pathPending == false && Vector3.Distance(_transform.position, currentTarget.position) <= distance)
            {
                Debug.Log(Vector3.Distance(_transform.position, currentTarget.position).ToString());

                agent.isStopped = true;

                status = BTstatus.SUCCESS;

                yield break;
            }
            
            yield return new WaitForSeconds(updateInterval);
                    
        }

        status = BTstatus.FAILURE;

        yield break;

    }
}
