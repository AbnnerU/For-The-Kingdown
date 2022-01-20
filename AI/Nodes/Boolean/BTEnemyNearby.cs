using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTEnemyNearby : BTnode
{
    private Transform startPoint;

    private Transform _transform;

    private LayerMask layerMask;

    private float radius;

    private string targetTag;

    private int maxAllocation;

    private Collider[] hitResults;

    public BTEnemyNearby(Transform objTrsform,Transform startPoint, LayerMask layerMask, float radius, string targetTag, int maxAllocation)
    {
        this.startPoint = startPoint;
        this.layerMask = layerMask;
        this.radius = radius;
        this.targetTag = targetTag;
        this.maxAllocation = maxAllocation;

        _transform = objTrsform;

        hitResults = new Collider[maxAllocation];
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        int collisioNumbers = Physics.OverlapSphereNonAlloc(startPoint.position, radius, hitResults, layerMask);

        if (collisioNumbers > 0)
        {
            for(int i = 0; i < collisioNumbers; i++)
            {
                if (hitResults[i].gameObject.transform == _transform)
                    continue;

                if (hitResults[i].gameObject.CompareTag(targetTag))
                {
                    status = BTstatus.SUCCESS;
                    yield break;
                }
            }        
        }

        status = BTstatus.FAILURE;
        yield break;


    }
}
