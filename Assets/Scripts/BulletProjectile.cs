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

        public bool isTargeting = false;
        private Transform target;
        private Vector3 forwardDir;
        private float timer = 0;

        private void Awake()
        {
            DestroyPrefabs();
        }

        private void Start()
        {
            if (isTargeting == true) 
            {
                target = findEnemy();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (isTargeting == false || target == null) { NonTargeting(); }
            if (isTargeting == true && target != null) 
            {
                timer += Time.deltaTime * 10f;
                forwardDir = transform.forward;
                Targeting(); 
            }
        }

        private void NonTargeting()
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        private void Targeting()
        {
            transform.position += forwardDir * Speed * Time.deltaTime;

            Vector3 targetDir = target.position - transform.position;
            float Dir = Vector3.Distance(transform.position, target.position);

            if (Dir <= 1.0f)//�Ÿ�1�̸� �ٷα׳� Ÿ�ٹ��� ȸ��
            {
                transform.LookAt(target);
            }
            if (targetDir != Vector3.zero && Dir > 1.0f)//���������� õõ�� ȸ��
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, timer * Time.deltaTime);
            }
        }

        private Transform findEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Transform closestEnemy = null;
            float closestDir = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float dir = Vector3.Distance(transform.position, enemy.transform.position);
                if (dir < closestDir)
                {
                    closestDir = dir;
                    closestEnemy = enemy.transform;
                }
            }
            return closestEnemy;
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
