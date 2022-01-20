using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTAtackEnemy : BTnode
{
    private IMarkTarget aiMarkedEnemy;

    private IAtackAI aiAtack;

    private NavMeshAgent agent;

    private float distance;

    private float updateInterval;

    private Transform _transform;

    private Transform currentTarget;

    private bool stopWalkWhenAtack;

    public BTAtackEnemy(IMarkTarget aICombat, IAtackAI aiAtack, NavMeshAgent agent, float distance, float updateInterval, bool stopWalkWhenAtack)
    {
        this.aiMarkedEnemy = aICombat;
        this.agent = agent;
        this.updateInterval = updateInterval;
        this.aiAtack = aiAtack;
        this.stopWalkWhenAtack = stopWalkWhenAtack;

        this.distance = distance;

        _transform = agent.transform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        currentTarget = aiMarkedEnemy.GetMarkedTarget();

        Collider targetCollider = currentTarget.GetComponent<Collider>();

        do
        {
            if(agent.enabled==false || agent == null)
            {
                status = BTstatus.FAILURE;

                yield break;
            }


            agent.SetDestination(currentTarget.position);

            yield return null;

            if (agent.enabled == false || agent == null)
            {
                status = BTstatus.FAILURE;

                yield break;
            }

            //Debug.Log(Vector3.Distance(_transform.position, currentTarget.position).ToString());
            //Debug.Log(agent.isStopped.ToString());

            if (currentTarget.gameObject.activeSelf == false || currentTarget==null )
            {
                status = BTstatus.SUCCESS;

                yield break;
            }


            if (agent.pathPending == false)
            {              
                if (Vector3.Distance(_transform.position, currentTarget.position) <= distance)
                {
                    //Debug.Log(Vector3.Distance(_transform.position, currentTarget.position));
                    agent.isStopped = true;

                    TryAtack();
                }
                else
                {
                    if(stopWalkWhenAtack && aiAtack.GetCanAtack()==true)
                        agent.isStopped = false;
                    else if(stopWalkWhenAtack==false)
                        agent.isStopped = false;
                }
            }

            yield return null;

        } while (aiMarkedEnemy.GetMarkedTarget() == currentTarget && currentTarget.gameObject.activeSelf == true && targetCollider.enabled ==true);


        status = BTstatus.FAILURE;

        yield break;

    }

    private void TryAtack()
    {
        if (aiAtack.GetCanAtack())
        {
            Quaternion lookRotation = Quaternion.LookRotation(currentTarget.position - _transform.position);
            lookRotation.x = 0;
            lookRotation.z = 0;

            _transform.rotation = lookRotation;

            aiAtack.DoAtack();
        }
    }
}


