using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTChangeColliderState : BTnode
{
    private Collider collider;

    private bool enabled;

    public BTChangeColliderState(Collider collider, bool enabled)
    {
        this.collider = collider;
        this.enabled = enabled;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (collider != null)
        {
            collider.enabled = enabled;

            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }

        yield break;
        
    }
}
