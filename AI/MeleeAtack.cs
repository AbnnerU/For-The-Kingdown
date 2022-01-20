using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAtack : AiAtack, IAtackAI
{
    [SerializeField] private bool drawGizmos;

    [SerializeField] private AtackType atackType;

    [SerializeField] private int atackDamage;

    [SerializeField] private Transform atackPoint;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private float radius;

    [SerializeField] private string targetTag;

    [SerializeField] private int maxAllocation;

    private bool canAtack = true;

    private Collider[] hitResults;

    private void Awake()
    {
        hitResults = new Collider[maxAllocation];
    }

    public void DoAtack()
    {
        if (canAtack)
        {
            OnAtack?.Invoke();

            canAtack = false;
        }
    }

    public void StartAtackArea()
    {
        OnExecuteAtack?.Invoke();

        int hitsAmount = Physics.OverlapSphereNonAlloc(atackPoint.position, radius, hitResults, layerMask);

        if (hitsAmount > 0)
        {
            for(int i = 0; i < hitsAmount; i++)
            {
                IDamageble damageble = hitResults[i].gameObject.GetComponent<IDamageble>();

                if (hitResults[i].CompareTag(targetTag) && damageble != null)
                {
                    print(hitResults[i].gameObject.name);
                    damageble.OnHit(atackDamage);

                    if (atackType == AtackType.SINGLE)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            print("Errou");
        }
    }

    public void CanAtack()
    {
        canAtack = true;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (atackPoint!=null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(atackPoint.position, radius);
            }
        }
    }

    public bool GetCanAtack()
    {
        return canAtack;
    }
}
