using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]public class OnShoot: UnityEvent { }

public class Turret : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;

    //[SerializeField] private float turnSpeed;

    [SerializeField] private bool active = false;

    [SerializeField] private float updateInterval;

    [SerializeField] private int maxAllocation;

    [SerializeField] private Transform verificationStartPoint;

    [SerializeField] private float verificationRadius;

    [SerializeField] private LayerMask layerMask;

    [Header("Projectile")]

    [SerializeField] private int atackDamage;

    [SerializeField] private float fireRate;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private float gravityValue = Physics.gravity.y;

    [SerializeField] private float projectileRadius;

    [SerializeField] private Transform atackPoint;

    [SerializeField] private GameObject projectilePrefab;

    public OnShoot onShoot;

    private Collider[] hitResults;

    private Transform _transform;

    private HealthManager targetHealth;

    private Transform targetTransform;

    private bool findedTarget = false;

    private bool canAtack=true;


    private void Awake()
    {
        _transform = GetComponent<Transform>();

        hitResults = new Collider[maxAllocation];
    }

    public void TurrentActive()
    {
        if (active == false)
        {
            active = true;
            StartCoroutine(StartTurret());
        }
    }

    public void StopTurrent()
    {
        active = false;
    }

    IEnumerator StartTurret()
    {
        do
        {
            if (findedTarget == false)
            {
                SeachForTarget();
            }
            else
            {
                if ((targetTransform.position - _transform.position).magnitude <= verificationRadius)
                {
                    if (canAtack)
                    {
                        if (targetHealth.IsAlive())
                        {
                            Shoot();

                            StartCoroutine(FireRate());
                        }
                        else
                        {
                            findedTarget = false;
                        }
                    }
                }
                else
                {
                    findedTarget = false;
                }
            }

            yield return new WaitForSeconds(updateInterval);

        } while (active);

        yield break;
    }

    IEnumerator FireRate()
    {
        canAtack = false;

        yield return new WaitForSeconds(fireRate);

        canAtack = true;

        yield break;
    }


    private void SeachForTarget()
    {
        int hitAmount = Physics.OverlapSphereNonAlloc(verificationStartPoint.position, verificationRadius, hitResults, layerMask);

        if (hitAmount > 0)
        {
            for (int i = 0; i < hitAmount; i++)
            {
                HealthManager healthManager = hitResults[i].gameObject.GetComponent<HealthManager>();

                if (healthManager)
                {
                    if (healthManager.IsAlive())
                    {
                        targetHealth = healthManager;
                        targetTransform = hitResults[i].transform;
                        findedTarget = true;
                        break;
                    }
                }
            }
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke();

        _transform.rotation = Quaternion.LookRotation(targetTransform.position - _transform.position);

        Vector3 speed = (atackPoint.transform.forward * projectileSpeed);

        GameObject obj = PoolManager.SpawnObject(projectilePrefab, atackPoint.position, Quaternion.identity);

        Projectile projectile = obj.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.EnableProjectile(atackPoint.position, speed, Vector3.up * gravityValue, projectileRadius, atackDamage);
        }
        else
        {
            print("Object dont have Projectile script");
        }
    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            if (verificationStartPoint)
            {
                Gizmos.DrawSphere(verificationStartPoint.position, verificationRadius);
            }
        }
    }
}
