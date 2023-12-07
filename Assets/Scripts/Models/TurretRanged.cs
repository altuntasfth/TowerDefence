using System;
using Components;
using Models.Abstractions;
using UnityEngine;

namespace Models
{
    public class TurretRanged : TurretBase
    {
        public float fireRate = 2f;
        private float nextTimeToShoot;
        
        private void FixedUpdate()
        {
            if (TargetInAttackRange())
            {
                Attack();
            }
        }

        protected override void Attack()
        {
            if (Time.time > nextTimeToShoot)
            {
                GameObject bulletObject = objectPool.GetPooledObject().gameObject;

                if (bulletObject == null)
                    return;
                
                bulletObject.GetComponent<Bullet>().damage += damageMultiplier;
                bulletObject.transform.SetPositionAndRotation(turretHead.position, turretHead.rotation);
                bulletObject.SetActive(true);

                nextTimeToShoot = Time.time + (fireRate / speedMultiplier);
            }
        }
    }
}