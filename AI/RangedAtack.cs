using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAtack : AiAtack, IAtackAI
{
    [SerializeField] private int atackDamage;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private float gravityValue = Physics.gravity.y;

    [SerializeField] private float projectileRadius;

    [SerializeField] private Transform atackPoint;

    [SerializeField] private GameObject projectilePrefab;

    private bool canAtack = true;

    public void DoAtack()
    {
        if (canAtack)
        {
            OnAtack?.Invoke();

            canAtack = false;
        }
    }

    public bool GetCanAtack()
    {
        return canAtack;
    }

    public void CanAtack()
    {
        canAtack = true;
    }


    public void Shoot()
    {
        OnExecuteAtack?.Invoke();

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
}
