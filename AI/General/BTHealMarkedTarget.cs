using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTHealMarkedTarget : BTnode
{
    private IHealer healer;

    private IMarkTarget aiMarkAlly;

    private IGenericMethod genericMethod;

    private NavMeshAgent agent;

    private Transform _transform;

    private int healvalue;

    public BTHealMarkedTarget(IHealer healer,IMarkTarget aiMarkAlly, IGenericMethod genericMethod, int healvalue, NavMeshAgent agent)
    {
        this.healer = healer;
        this.aiMarkAlly = aiMarkAlly;
        this.genericMethod = genericMethod;
        this.healvalue = healvalue;
        this.agent = agent;

        _transform = agent.transform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (aiMarkAlly.GetMarkedTarget().gameObject.activeSelf == true)
        {
            HealthBasics health = aiMarkAlly.GetMarkedTarget().gameObject.GetComponent<HealthBasics>();

            if (health != null && healer.GetCanHeal() && agent.enabled ==true)
            {
                health.Heal(healvalue);

                genericMethod.Action();

                agent.isStopped = true;

                Quaternion lookRotation = Quaternion.LookRotation(aiMarkAlly.GetMarkedTarget().transform.position - _transform.position);
                lookRotation.x = 0;
                lookRotation.z = 0;

                _transform.rotation = lookRotation;

                status = BTstatus.SUCCESS;

                yield break;
            }
        }

        status = BTstatus.FAILURE;
        yield break;
    }
}
