using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTIsColliderEnabled : BTnode
{
    private Collider collider;

    public BTIsColliderEnabled(Collider collider)
    {
        this.collider = collider;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (collider != null)
        {
            if (collider.enabled == true)
            {
                status = BTstatus.SUCCESS;
            }
            else
            {
                status = BTstatus.FAILURE;
            }
        }
        else
        {
            status = BTstatus.FAILURE;
        }

        yield break;

    }
}

