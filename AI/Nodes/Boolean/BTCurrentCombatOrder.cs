using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTCurrentCombatOrder : BTnode
{
    private AIGeneral AI;

    private CombatOrder reference;

    public BTCurrentCombatOrder(CombatOrder reference, AIGeneral AI)
    {
        this.AI = AI;

        this.reference = reference;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        if (AI.GetCombatOrder() == reference)
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
