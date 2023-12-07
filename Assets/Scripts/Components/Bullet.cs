using System;
using System.Collections;
using Models.Abstractions;
using Pooling;
using UnityEngine;

namespace Components
{
    public class Bullet : MonoBehaviour
    {
        public float damage = 1f;
        [SerializeField] private float timeoutDelay = 3f;
        [SerializeField] private PooledObject pooledObject;

        private void Update()
        {
            transform.position += transform.forward * (Time.deltaTime * 10f);
        }

        private void OnEnable()
        {
            Deactivate(timeoutDelay);
        }

        public void Deactivate(float delay)
        {
            StartCoroutine(DeactivateRoutine(delay));
        }

        private void OnTriggerEnter(Collider other)
        {
            var characterBase = other.gameObject.GetComponent<CharacterBase>();
            if (characterBase != null)
            {
                characterBase.TakeDamage(1);
                Deactivate(0f);
            }
        }

        IEnumerator DeactivateRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);

            damage = 1f;
            Rigidbody rBody = GetComponent<Rigidbody>();
            rBody.velocity = new Vector3(0f, 0f, 0f);
            rBody.angularVelocity = new Vector3(0f, 0f, 0f);

            pooledObject.Release();
            gameObject.SetActive(false);
        }
    }
}