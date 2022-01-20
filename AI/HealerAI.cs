using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerAI : AIGeneral, IMarkTarget, IGenericMethod,IHealer, IDefensePoint
{ 
    [Header("Rotation on defesePoint")]

    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float enemyNearbyOfPointVerification;

    [Header("Retreat")]

    [SerializeField] private string retreatPoint;

    [Header("Nearby Ally")]

    [SerializeField] private bool drawGizmos;

    [SerializeField] private Transform markedAlly;

    [SerializeField] private Transform verificationStartPoint;

    [SerializeField] private float verificationRadius;

    [SerializeField] private string targetTag;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int maxAllocation;

    [Header("Nearby Enemy")]

    [SerializeField] private float enemyVerificationRadius;

    [SerializeField] private string enemyTag;

    [SerializeField] private LayerMask enemyLayerMask;


    [Header("Enemy base")]

    [SerializeField] private string enemyBase;

    [SerializeField] private float minDistance;

    [Header("Heal")]

    [SerializeField] private float healRechargeTime;

    [SerializeField] private int healvalue;

    private BTSelector rootSelector;

    private IAtackAI atackAI;

    private Transform enemyBaseTransform;

    private Transform retreatPointTransform;

    private bool canHeal=true;


    private void Awake()
    {        
        //ConstructBehaviorTree();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        canHeal = true;
    }

    private void OnDisable()
    {
        StopBehaviorTree();
    }

    public override void StopBehaviorTree()
    {
        behaviorTree.Stop();

        rootSelector.ForceBreak();

        agent.enabled = false;
    }

    private void ConstructBehaviorTree()
    {
        enemyBaseTransform = GameObject.FindGameObjectWithTag(enemyBase).GetComponent<Transform>();

        retreatPointTransform = GameObject.FindGameObjectWithTag(retreatPoint).GetComponent<Transform>();

        #region Retreat
        BTCurrentCombatOrder btIsRetreating = new BTCurrentCombatOrder(CombatOrder.RETREAT, this);
        BTMoveToPoint bTMoveToRetreatpoint = new BTMoveToPoint(this, agent, retreatPointTransform,3);
        BTConditionalSequence btRetreatCondicionalSequence = new BTConditionalSequence(new List<BTnode> { btIsRetreating }, bTMoveToRetreatpoint);
        BTChangeColliderState btDisableCollder = new BTChangeColliderState(objCollider, false);
        BTSequence btRetreatSequence = new BTSequence(new List<BTnode>
        {
            btIsRetreating,
            btRetreatCondicionalSequence,
            btDisableCollder
        });
        #endregion


        #region EnableCollider
        BTIsColliderEnabled btIsColliderEnabled = new BTIsColliderEnabled(objCollider);
        BTInverter btIsColliderEnabledInverter = new BTInverter(btIsColliderEnabled);
        BTCurrentCombatOrder btIsDefending = new BTCurrentCombatOrder(CombatOrder.DEFEND, this);
        BTCurrentCombatOrder btIsAtacking = new BTCurrentCombatOrder(CombatOrder.ATACK, this);
        BTSelector btStateSelector = new BTSelector(new List<BTnode> { btIsDefending, btIsAtacking });
        BTChangeColliderState btEnableCollider = new BTChangeColliderState(objCollider, true);
        BTSequence btEnableColliderSequence = new BTSequence(new List<BTnode>
        {
            btIsColliderEnabledInverter,
            btStateSelector,
            btEnableCollider
        });
        #endregion


        #region Defend Sequence
        BTIsOnDefensePoint btIsOnDefensePoint = new BTIsOnDefensePoint(this, agent);
        BTInverter btIsOnDefensePointInverter = new BTInverter(btIsOnDefensePoint);
        BTGoToDefensePoint btGoToDefensePoint = new BTGoToDefensePoint(this, agent);
        BTConditionalSequence bTConditionalSequenceDefending = new BTConditionalSequence(new List<BTnode> { btIsDefending }, btGoToDefensePoint);
        BTSequence btBackToDefenseSequence = new BTSequence(new List<BTnode>
        {
            btIsDefending,
            btIsOnDefensePointInverter,
            bTConditionalSequenceDefending
        });
        #endregion


        #region Rotate on defense
        BTIsOnRotation btIsOnRotationDefense = new BTIsOnRotation(transform, targetRotation);
        BTInverter btIsOnRotationDefenseInverter = new BTInverter(btIsOnRotationDefense);
        BTRotateTo btDefenseRotation = new BTRotateTo(agent, targetRotation, rotationSpeed);
        BTConditionalSequence btConditionalSequenceRotate = new BTConditionalSequence(new List<BTnode> { btIsDefending }, btDefenseRotation);
        BTSequence btDefenseRotationSequence = new BTSequence(new List<BTnode>
        {
            btIsDefending,
            btIsOnDefensePoint,
            btIsOnRotationDefenseInverter,
            btConditionalSequenceRotate
        });
        #endregion


        #region Defend Point
        BTEnemyNearby btEnemyNearbyOfDefendPoint = new BTEnemyNearby(transform, verificationStartPoint, enemyLayerMask, enemyNearbyOfPointVerification, enemyTag, maxAllocation);
        BTEnemyNearby btAllyNearby = new BTEnemyNearby(transform, verificationStartPoint, layerMask, verificationRadius, targetTag, maxAllocation);
        BTMarkLowestHealth btMarkLowestHealth = new BTMarkLowestHealth(this, transform, verificationStartPoint, layerMask, verificationRadius, targetTag, maxAllocation);
        BTHealMarkedTarget btHealMarkedTarget = new BTHealMarkedTarget(this, this, this, healvalue, agent);
        BTSequence btDefendPointSequence = new BTSequence(new List<BTnode>
        {
            btIsDefending,
            btIsOnDefensePoint,
            btIsOnRotationDefense,
            //btEnemyNearbyOfDefendPoint,
            btAllyNearby,
            btMarkLowestHealth,
            btHealMarkedTarget

        });
        #endregion


        #region Heal       
        BTSequence btHealSequence = new BTSequence(new List<BTnode>
        {
            btIsAtacking,
            btAllyNearby,
            btMarkLowestHealth,
            btHealMarkedTarget
        });
        #endregion


        #region Retreat   
        BTEnemyNearby btEnemyNearby = new BTEnemyNearby(transform, verificationStartPoint, enemyLayerMask, enemyVerificationRadius, enemyTag, maxAllocation);
        BTInverter btNearbyAllyInverter = new BTInverter(btAllyNearby);
        BTConditionalSequence btEnemyNearbyCondicionalSequence = new BTConditionalSequence(new List<BTnode> { btIsAtacking, btNearbyAllyInverter }, btGoToDefensePoint);
        BTSequence btEnemyNearbySequence = new BTSequence(new List<BTnode>
        {
            btIsAtacking,
            btNearbyAllyInverter,
            btEnemyNearby,
            btIsOnDefensePointInverter,
            btEnemyNearbyCondicionalSequence
        });
        #endregion


        #region Atack base
        BTInverter btEnemyNearbyInverter = new BTInverter(btEnemyNearby);
        BTMarktarget btMarkBase = new BTMarktarget(enemyBaseTransform, this);
        //BTInverter btNearbyAllyInverter = new BTInverter(btAllyNearby);
        BTMoveToPoint btMoveToEnemyBase = new BTMoveToPoint(this, agent, enemyBaseTransform, minDistance);
        BTConditionalSequence btAtackBaseCondicionalSequence = new BTConditionalSequence(new List<BTnode> { btIsAtacking, btNearbyAllyInverter,btEnemyNearbyInverter }, btMoveToEnemyBase);
        BTSequence btAtackBaseSequence = new BTSequence(new List<BTnode>
        {
            btIsAtacking,
            btNearbyAllyInverter,
            btEnemyNearbyInverter,
            btMarkBase,
            btAtackBaseCondicionalSequence
        });
        #endregion


        //----Root----
         rootSelector = new BTSelector(new List<BTnode>
        {
             btRetreatSequence,
             btEnableColliderSequence,
             btDefendPointSequence,
             btBackToDefenseSequence,
             btDefenseRotationSequence,           
             btHealSequence,
             btEnemyNearbySequence,
             btAtackBaseSequence
        });

        behaviorTree.SetActive(true);

        behaviorTree.SetBehaviorRoot(rootSelector);

        behaviorTree.SetExecutionInterval(executionInterval);

        behaviorTree.StartCoroutine(behaviorTree.Begin());
    }

    public override void StartBehaviorTree()
    {
        agent.enabled = true;
        if (alreadyConstructBehaviorTree == false)
        {
            ConstructBehaviorTree();
        }
        else
        {
            behaviorTree.SetActive(true);
            behaviorTree.StartCoroutine(behaviorTree.Begin());
        }
    }

    public void MarkTarget(Transform reference)
    {
        markedAlly = reference;
    }

    public Transform GetMarkedTarget()
    {
        return markedAlly;
    }

    public bool GetCanHeal()
    {
        return canHeal;
    }

    public void Action()
    {
        OnAction?.Invoke();

        StartCoroutine(Recharge());
    }

    IEnumerator Recharge()
    {
        canHeal = false;
        yield return new WaitForSeconds(healRechargeTime);
        canHeal = true;
    }

    public void SetDefensePoint(Transform point)
    {
        defesePoint = point;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (verificationStartPoint)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(verificationStartPoint.position, verificationRadius);
            }
        }
    }

    
}
