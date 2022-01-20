using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BTIsOnRotation : BTnode
{
    private Transform _transform;

    private Vector3 rotationRef;

    public BTIsOnRotation(Transform transform, Vector3 rotationRef)
    {
        _transform = transform;

        this.rotationRef = rotationRef;      
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if(_transform.eulerAngles == rotationRef)
        {
            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }
        //Debug.Log("Esta na rotção :" + status.ToString());
        yield break;
    }
}
