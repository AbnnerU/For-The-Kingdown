using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;

    [SerializeField] private bool active;

    [SerializeField] private Vector3 gravityValue = Physics.gravity;

    [SerializeField] private Vector3 projectileVelocity;

    [SerializeField] private int damage;

    [SerializeField] private string targetTag;

    [SerializeField] private LayerMask layerMask;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private int predictionPerFrame=6;

    [SerializeField] private float projectileRadius;

    [SerializeField] private int maxAllocation;

    private RaycastHit[] hitResults;

    private Transform _transform;

    private Vector3 point1;


    private void Awake()
    {
        hitResults = new RaycastHit[maxAllocation];

        _transform = GetComponent<Transform>();
    }

    private void OnDisable()
    {
        active = false;
    }

    private void Update()
    {
        if (active == false)
            return;

        if (active)
        {
            float currentDeltaTime = Time.deltaTime;

            point1 = transform.position;

            float totalPredictions = 1f / predictionPerFrame;

            for (float i = 0; i < 1f; i += totalPredictions)
            {
                projectileVelocity += gravityValue * (totalPredictions * currentDeltaTime);

                Vector3 point2 = point1 + projectileVelocity * (totalPredictions * currentDeltaTime);

                Vector3 direction = point2 - point1;

                int hits = Physics.SphereCastNonAlloc(point1, projectileRadius, direction.normalized, hitResults, direction.magnitude, layerMask);

                if (hits > 0)
                {
                    Collider collider = hitResults[0].collider;

                    if (collider != null)
                    {
                        if (collider.gameObject.layer == groundLayer)
                        {
                            PoolManager.ReleaseObject(gameObject);
                            break;
                        }
                        else 
                        {
                            OnHit(collider.gameObject);
                            PoolManager.ReleaseObject(gameObject);

                            break;
                        }
                    }
                }

                point1 = point2;

            }

            _transform.position = point1;

        }
    }


    public void EnableProjectile(Vector3 startPoint,Vector3 speedOrietation,Vector3 gravityValue,float radius,int damage)
    {
        point1 = startPoint;

        projectileVelocity = speedOrietation;

        this.gravityValue = gravityValue;

        projectileRadius = radius;

        this.damage = damage;

        active = true;
    }

    private void OnHit(GameObject obj)
    {
        IDamageble damageble = obj.GetComponent<IDamageble>();

        if (damageble != null && obj.CompareTag(targetTag))
        {
            damageble.OnHit(damage);         
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.DrawSphere(transform.position, projectileRadius);
        }
        
    }
}
