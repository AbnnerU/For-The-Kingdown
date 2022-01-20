using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTRotateTo : BTnode
{
    private Vector3 targetRotation;

    private float rotationSpeed;

    private NavMeshAgent agent;

    private Transform _tranform;


    public BTRotateTo(NavMeshAgent agent, Vector3 targetRotation, float rotationSpeed)
    {
        this.agent = agent;
        this.targetRotation = targetRotation;
        this.rotationSpeed = rotationSpeed;

        _tranform = agent.transform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        do
        {
            _tranform.rotation = Quaternion.RotateTowards(_tranform.rotation, Quaternion.Euler(targetRotation), rotationSpeed*Time.deltaTime);

            yield return null;

        } while (_tranform.eulerAngles != targetRotation);


        if(_tranform.eulerAngles == targetRotation)
        {
            status = BTstatus.SUCCESS;
        }
        else
        {
            status = BTstatus.FAILURE;
        }
        yield break;
    }
}
