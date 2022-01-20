using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMarktarget : BTnode
{
    private IMarkTarget aiMarkEnemy;

    private Transform target;

    public BTMarktarget(Transform target, IMarkTarget aiMarkEnemy)
    {
        this.target = target;
        this.aiMarkEnemy = aiMarkEnemy;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if(aiMarkEnemy!=null && target!=null && target.gameObject.activeSelf == true)
        {
            aiMarkEnemy.MarkTarget(target);

            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }

        yield break;
    }
}
