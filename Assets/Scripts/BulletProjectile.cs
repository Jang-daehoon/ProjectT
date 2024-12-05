using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoonsCodes
{
    public class BulletProjectile : MonoBehaviour
    {
        public float Damage;
        public float Speed;
        public float Lifetime = 3f;        // �Ѿ��� ���� �ð� (��)
        public bool isEnhanced;           // ��ȭ ���� Ȯ��
        public float EnhancedRadius = 10f; // ��ȭ�� ���� �ݰ�
        public ParticleSystem HitParticle;

        private void Awake()
        {
            DestroyPrefabs();
        }

        // Update is called once per frame
        void Update()
        {
            // �Ѿ��� �������� �̵�
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                // �浹�� ������ ������ ����
                ApplyDamage(other);

                // ��ȭ ������ ��� ���� ���� ����
                if (isEnhanced)
                {
                    PerformEnhancedAttack();
                }

                // �Ѿ� �ı�
                Destroy(gameObject);
            }
        }

        private void ApplyDamage(Collider enemy)
        {
            // �浹�� ������ ������ ���� �� ��ƼŬ ����
            ParticleSystem hitParticle = Instantiate(HitParticle, enemy.transform.position, Quaternion.identity);
            hitParticle.Play();

            enemy.GetComponent<ITakeDamage>()?.TakeDamage(Damage);
        }

        private void PerformEnhancedAttack()
        {
            // OverlapSphere�� ����Ͽ� ���� �� �� Ž��
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, EnhancedRadius);

            foreach (Collider hitEnemy in hitEnemies)
            {
                if (hitEnemy.CompareTag("Enemy"))
                {
                    // ���� �� ������ ������ ����
                    ApplyDamage(hitEnemy);
                }
            }
        }

        private void DestroyPrefabs()
        {
            Destroy(gameObject, Lifetime);
        }

        private void OnDrawGizmosSelected()
        {
            // �����Ϳ��� ��ȭ ������ �ð������� ǥ��
            if (isEnhanced)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, EnhancedRadius);
            }
        }
    }
}
