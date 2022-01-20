using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAI : AIGeneral,IMarkTarget,IDefensePoint
{
   
    [Header("Rotation on defesePoint")]

    [SerializeField] private Vector3 targetRotation;

    [SerializeField] private float rotationSpeed;

    [SerializeField] private float maxDistanceDefending;

    [Header("Nearby Enemy")]

    [SerializeField] private bool drawGizmos;

    [SerializeField] private Transform markedEnemy;

    [SerializeField] private Transform verificationStartPoint;

    [SerializeField] private float verificationRadius;

    [SerializeField] private string targetTag;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private int maxAllocation;

    [Header("Retreat")]

    [SerializeField] private string retreatPoint;

    [Header("Enemy base")]

    [SerializeField] private string enemyBase;

    [Header("Atack")]

    [SerializeField] private float enemyPositionUpdateInterval;

    [SerializeField] private bool stopMoveWhenAtack;

    [SerializeField] private float atackDistance;

    [SerializeField] private GameObject atackScript;

    private BTSelector rootSelector;

    private IAtackAI atackAI;

    private Transform enemyBaseTransform;

    private Transform retreatPointTransform;
   
    private void Awake()
    {
        //ConstructBehaviorTree();
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

        atackAI = atackScript.GetComponent<IAtackAI>();

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
        BTEnemyNearby btEnemyNearby = new BTEnemyNearby(transform, verificationStartPoint, layerMask, verificationRadius, targetTag, maxAllocation);
        BTInverter btEnemyNearbyInverter = new BTInverter(btEnemyNearby);
        BTIsOnDefensePoint btIsOnDefensePoint = new BTIsOnDefensePoint(this, agent);
        BTInverter btIsOnDefensePointInverter = new BTInverter(btIsOnDefensePoint);
        BTGoToDefensePoint btGoToDefensePoint = new BTGoToDefensePoint(this, agent);
        BTConditionalSequence bTConditionalSequenceDefending = new BTConditionalSequence(new List<BTnode> { btIsDefending,btEnemyNearbyInverter }, btGoToDefensePoint);
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
        BTConditionalSequence btConditionalSequenceRotate = new BTConditionalSequence(new List<BTnode> { btIsDefending,btEnemyNearbyInverter }, btDefenseRotation);
        BTSequence btDefenseRotationSequence = new BTSequence(new List<BTnode>
        {
            btIsDefending,
            btIsOnDefensePoint,
            btIsOnRotationDefenseInverter,
            btConditionalSequenceRotate
        });
        #endregion


        #region Protect      
        BTMarkEnemy btMarkEnemy = new BTMarkEnemy(this, transform, verificationStartPoint, layerMask, verificationRadius, targetTag, maxAllocation);
        BTIsOnDefensePoint btIsNearbyOfDefensePoint = new BTIsOnDefensePoint(this, agent, maxDistanceDefending);
        BTAtackEnemy btAtackEnemy = new BTAtackEnemy(this, atackAI, agent, atackDistance, enemyPositionUpdateInterval, stopMoveWhenAtack);
        BTConditionalSequence btCondicionalSequenceDefendFromEnemys = new BTConditionalSequence(new List<BTnode> { btIsDefending, btIsNearbyOfDefensePoint }, btAtackEnemy);
        BTSequence btDefendPointSequence = new BTSequence(new List<BTnode>
        {
            btIsDefending,
            //btIsOnDefensePoint,
            //btIsOnRotationDefense,
            btIsNearbyOfDefensePoint,
            btEnemyNearby,
            btMarkEnemy,
            btCondicionalSequenceDefendFromEnemys
        });
        #endregion


        #region AtackEnemy          
        BTDiferentNearbyEnemy btDiferentNearbyEnemy = new BTDiferentNearbyEnemy(this, transform, verificationStartPoint, layerMask, verificationRadius, targetTag, maxAllocation);
        BTInverter btDiferentNearbyEnemyInverter = new BTInverter(btDiferentNearbyEnemy);      
        BTConditionalSequence btAtackEnemyConditionalSequence = new BTConditionalSequence(new List<BTnode> { btIsAtacking,btDiferentNearbyEnemyInverter }, btAtackEnemy);
        BTSequence btAtackEnemySequence = new BTSequence(new List<BTnode>
        {
            btIsAtacking,
            btEnemyNearby,
            btMarkEnemy,
            btAtackEnemyConditionalSequence,
            //btAtack
        });
        #endregion

        #region Atack base
       
        BTMarktarget btMarkBase = new BTMarktarget(enemyBaseTransform, this);
        BTMoveToPoint btMoveToMainTarget = new BTMoveToPoint(this, agent, enemyBaseTransform);
        BTConditionalSequence btAtackBaseCondicionalSequence = new BTConditionalSequence(new List<BTnode> { btIsAtacking, btEnemyNearbyInverter }, btAtackEnemy);
        BTSequence btAtackBaseSequence = new BTSequence(new List<BTnode>
        {
            btIsAtacking,
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
             btBackToDefenseSequence,
             btDefenseRotationSequence,
             btDefendPointSequence,
             btAtackEnemySequence,
             btAtackBaseSequence
        });

        behaviorTree.SetActive(true);

        behaviorTree.SetBehaviorRoot(rootSelector);

        behaviorTree.SetExecutionInterval(executionInterval);

        behaviorTree.StartCoroutine(behaviorTree.Begin());

        alreadyConstructBehaviorTree = true;
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
        markedEnemy = reference;
    }

    public Transform GetMarkedTarget()
    {
        return markedEnemy;
    }

    public void Atack()
    {
        atackAI.DoAtack();
    }

    public bool GetCanAtack()
    {
        return atackAI.GetCanAtack();
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
