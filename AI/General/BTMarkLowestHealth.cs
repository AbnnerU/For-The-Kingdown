using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTMarkLowestHealth : BTnode
{
    private IMarkTarget aiMarkAlly;

    private Transform startPoint;

    private LayerMask layerMask;

    private float radius;

    private string targetTag;

    private int maxAllocation;

    private Collider[] hitResults;

    private Transform _transform;


    public BTMarkLowestHealth(IMarkTarget aiMarkTarget, Transform aiTransform, Transform startPoint, LayerMask layerMask, float radius, string targetTag, int maxAllocation)
    {
        this.aiMarkAlly = aiMarkTarget;
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
            float lowestHealth = Mathf.Infinity;
            int id = 0;

            for (int i = 0; i < collisioNumbers; i++)
            {
                if (hitResults[i].gameObject.transform == _transform)
                    continue;

                GameObject temp = hitResults[i].gameObject;

                if (temp.CompareTag(targetTag))
                {
                    HealthBasics health = temp.GetComponent<HealthBasics>();

                    if (health != null)
                    {
                        if (health.GetCurrentHealth() < lowestHealth)
                        {
                            find = true;

                            lowestHealth = health.GetCurrentHealth();

                            id = i;
                        }
                    }
                }
            }

            if (find)
            {
                if (hitResults[id].gameObject.activeSelf == true)
                {

                    status = BTstatus.SUCCESS;

                    aiMarkAlly.MarkTarget(hitResults[id].gameObject.transform);

                    yield break;
                }
            }
        }

        status = BTstatus.FAILURE;
        yield break;
    }
}
