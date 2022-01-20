using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTnode
{
    private List<BTnode> nodes;

    private BTnode currentNode;

    private BehaviorTree bt;

    private bool forceBreak = false;

    public BTSelector(List<BTnode> nodes)
    {
        this.nodes = nodes;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        bt = behaviorTree;

        for(int i = 0 ; i < nodes.Count; i++)
        {
            if (forceBreak)
            {
                forceBreak = false;
                yield break;
            }

            currentNode = nodes[i];
            yield return behaviorTree.StartCoroutine(currentNode.Run(behaviorTree));

            if (forceBreak)
            {
                forceBreak = false;
                yield break;
            }

            if (currentNode.GetStatus() == BTstatus.SUCCESS)
            {
                status = BTstatus.SUCCESS;
                break;
            }
        }

        if (status == BTstatus.RUNNING)
            status = BTstatus.FAILURE;
        
    }

    public void ForceBreak()
    {
        if (currentNode != null)
        {
            bt.StopCoroutine(currentNode.Run(bt));
        }

        forceBreak = true;
    }
}
