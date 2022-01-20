using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMarkEnemy : BTnode
{
    private IMarkTarget aiMarkEnemy;

    private Transform startPoint;

    private LayerMask layerMask;

    private float radius;

    private string targetTag;

    private int maxAllocation;

    private Collider[] hitResults;

    private Transform _transform;

    public BTMarkEnemy(IMarkTarget aiMarkEnemy, Transform aiTransform ,Transform startPoint, LayerMask layerMask, float radius, string targetTag, int maxAllocation)
    {
        this.aiMarkEnemy = aiMarkEnemy;

        this.startPoint = startPoint;
        this.layerMask = layerMask;
        this.radius = radius;
        this.targetTag = targetTag;
        this.maxAllocation = maxAllocation;

        hitResults = new Collider[maxAllocation];

        _transform = aiTransform;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        int collisioNumbers = Physics.OverlapSphereNonAlloc(startPoint.position, radius, hitResults, layerMask);

        if (collisioNumbers > 0)
        {
            bool find = false; 
            float closer = Mathf.Infinity;
            int id = 0;

            for (int i = 0; i < collisioNumbers; i++)
            {
                if (hitResults[i].gameObject.transform == _transform)
                    continue;

                if (hitResults[i].gameObject.CompareTag(targetTag))
                {
                    float currentDistance = (hitResults[i].transform.position - _transform.position).magnitude;

                    if (currentDistance < closer)
                    {
                        find = true;

                        closer = currentDistance;

                        id = i;
                    }
                }
            }

            if (find)
            {
                if (hitResults[id].gameObject.activeSelf == true)
                {

                    status = BTstatus.SUCCESS;

                    aiMarkEnemy.MarkTarget(hitResults[id].transform);
                    yield break;
                }
            }
        }

        status = BTstatus.FAILURE;
        yield break;


    }
}
