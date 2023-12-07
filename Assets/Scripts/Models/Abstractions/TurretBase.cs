using System;
using System.Collections.Generic;
using Cinemachine.Utility;
using Managers;
using Pooling;
using UnityEngine;

namespace Models.Abstractions
{
    public abstract class TurretBase : MonoBehaviour
    {
        public LevelManager levelManager;
        public Transform turretHead;
        [SerializeField] protected ObjectPoolBase objectPool;
        
        public float damageMultiplier = 1f;
        public float speedMultiplier = 1f;
        public float attackRange = 5f;

        public int damageLevel;
        public int speedLevel;
        
        public int currentDamageCost = 2;
        public int currentSpeedCost = 2;
        
        private void Update()
        {
            RotateToTarget();
        }

        private void RotateToTarget()
        {
            var targetCharacter = GetTargetCharacter();
            if (targetCharacter == null)
            {
                return;
            }
            
            Vector3 directionToTarget = targetCharacter.transform.position.ProjectOntoPlane(Vector3.up) - 
                                        transform.position.ProjectOntoPlane(Vector3.up);
            Quaternion desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 2f);
        }

        protected abstract void Attack();

        protected bool TargetInAttackRange()
        {
            if (GetTargetCharacter() == null)
            {
                return false;
            }

            var distanceToTarget = Vector3.Distance(transform.position.ProjectOntoPlane(Vector3.up), 
                GetTargetCharacter().transform.position.ProjectOntoPlane(Vector3.up));
            
            return (distanceToTarget <= attackRange);
        }
        
        private CharacterBase GetTargetCharacter()
        {
            CharacterBase targetCharacter = null;
            List<CharacterBase> targetCharacters = levelManager.enemyCharacters;;

            float minDistance = float.MaxValue;
            for (var i = 0; i < targetCharacters.Count; i++)
            {
                float distance = Vector3.Distance(transform.position.ProjectOntoPlane(Vector3.up), 
                    targetCharacters[i].transform.position.ProjectOntoPlane(Vector3.up));
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetCharacter = targetCharacters[i];
                }
            }
            
            return targetCharacter;
        }
    }
}