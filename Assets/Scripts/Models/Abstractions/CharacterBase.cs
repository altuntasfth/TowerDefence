using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace Models.Abstractions
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public enum CharacterType
        {
            ENEMY,
            FRIEND
        };
        
        public enum AttackType
        {
            MELEE,
            RANGED,
            BOMBER
        };

        public LevelManager levelManager;
        public AttackType attackType;
        [SerializeField] protected CharacterType characterType;
        [SerializeField] protected NavMeshAgent navMeshAgent;
        [SerializeField] protected float attackRange;
        [SerializeField] protected float checkTargetInAttackRangeDelay = 1.5f;
        public float health = 2f;

        protected List<CharacterBase> targetCharacters = new List<CharacterBase>();
        
        private bool isDead = false;
        
        public void SetCharacterType(CharacterType characterType)
        {
            this.characterType = characterType;
        }
        
        public void SetDestination(Vector3 destination)
        {
            navMeshAgent.SetDestination(destination);
        }

        private void Update()
        {
            RotateToTarget();

            if (TargetInAttackRange())
            {
                navMeshAgent.SetDestination(transform.position);
                
                if (GetTargetCharacter() != null)
                {
                    //GetTargetCharacter().TakeDamage(attackType);
                }
            }
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
            {
                return;
            }
            
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                levelManager.enemyCharacters.Remove(this);
                levelManager.UpdateUI();
                
                if (levelManager.enemyCharacters.Count == 0)
                {
                    levelManager.SpawnWave();
                }
                Destroy(this.gameObject);
            }
        }

        private void RotateToTarget()
        {
            Vector3 directionToTarget = levelManager.tower.position.ProjectOntoPlane(Vector3.up) - 
                                        transform.position.ProjectOntoPlane(Vector3.up);
            Quaternion desiredRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, 0.5f);
        }

        private bool TargetInAttackRange()
        {
            //var distanceToTarget = Vector3.Distance(transform.position.ProjectOntoPlane(Vector3.up), 
              //  GetTargetCharacter().transform.position.ProjectOntoPlane(Vector3.up));
              
            var distanceToTarget = Vector3.Distance(transform.position.ProjectOntoPlane(Vector3.up), 
                levelManager.tower.position.ProjectOntoPlane(Vector3.up));
            
            return (distanceToTarget <= attackRange);
        }
        
        private CharacterBase GetTargetCharacter()
        {
            CharacterBase targetCharacter = null;
            List<CharacterBase> targetCharacters = GetTargetCharacters();

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
        
        private List<CharacterBase> GetTargetCharacters()
        {
            if (characterType == CharacterType.FRIEND)
            {
                targetCharacters = levelManager.enemyCharacters;
            }
            else
            {
                targetCharacters = levelManager.playerCharacters;
            }

            return targetCharacters;
        }
    }
}