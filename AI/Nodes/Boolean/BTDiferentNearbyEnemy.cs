using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTDiferentNearbyEnemy : BTnode
{
    private IMarkTarget aiMarkEnemy;

    private Transform startPoint;

    private Transform _transform;

    private LayerMask layerMask;

    private float radius;

    private string targetTag;

    private int maxAllocation;

    private Collider[] hitResults;

    public BTDiferentNearbyEnemy(IMarkTarget aiMarkEnemy, Transform aiTransform, Transform startPoint, LayerMask layerMask, float radius, string targetTag, int maxAllocation)
    {
        this.aiMarkEnemy = aiMarkEnemy;
        this.startPoint = startPoint;
        this.layerMask = layerMask;
        this.radius = radius;
        this.targetTag = targetTag;
        this.maxAllocation = maxAllocation;

        _transform = aiTransform;

        hitResults = new Collider[maxAllocation];
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        List<Transform> tempObj = new List<Transform>();

        Transform currentTarget = aiMarkEnemy.GetMarkedTarget();

        Vector3 position = _transform.position;
 

        int collisioAmount = Physics.OverlapSphereNonAlloc(startPoint.position, radius, hitResults, layerMask);

        if (collisioAmount > 0)
        {
            for (int i = 0; i < collisioAmount; i++)
            {
                if (hitResults[i].gameObject.transform == _transform)
                    continue;

                if (hitResults[i].gameObject.CompareTag(targetTag))
                {
                    tempObj.Add(hitResults[i].gameObject.transform);                                   
                }
            }
        }
        else
        {
            status = BTstatus.FAILURE;
            yield break;
        }

        int count = tempObj.Count;

        int selectedObj = -1;

        float closer = (currentTarget.position - position).magnitude;

        if (count > 0)
        {
            for(int i = 0; i < count; i++)
            {
                if (tempObj[i] == currentTarget)
                    continue;
                else
                {
                    float currentdistance = (tempObj[i].position - position).magnitude;

                    if (currentdistance < closer)
                    {
                        selectedObj = i;
                        closer = currentdistance;
                    }
                }
            }

            if (selectedObj != -1)
            {
                status = BTstatus.SUCCESS;
                yield break;
            }

        }


        status = BTstatus.FAILURE;
        yield break;
    }
}
    

