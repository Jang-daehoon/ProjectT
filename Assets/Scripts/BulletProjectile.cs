using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HoonsCodes
{
    public class BulletProjectile : MonoBehaviour
    {
        public float Damage;
        public float Speed;
        public float Lifetime = 3f;        // 총알의 생명 시간 (초)
        public bool isEnhanced;           // 강화 여부 확인
        public float EnhancedRadius = 10f; // 강화된 범위 반경
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

            if (Dir <= 1.0f)//거리1이면 바로그냥 타겟방향 회전
            {
                transform.LookAt(target);
            }
            if (targetDir != Vector3.zero && Dir > 1.0f)//적방향으로 천천히 회전
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
                // 충돌한 적에게 데미지 전달
                ApplyDamage(other);

                // 강화 상태일 경우 범위 공격 실행
                if (isEnhanced)
                {
                    PerformEnhancedAttack();
                }

                // 총알 파괴
                Destroy(gameObject);
            }
        }

        private void ApplyDamage(Collider enemy)
        {
            // 충돌한 적에게 데미지 전달 및 파티클 생성
            ParticleSystem hitParticle = Instantiate(HitParticle, enemy.transform.position, Quaternion.identity);
            hitParticle.Play();

            enemy.GetComponent<ITakeDamage>()?.TakeDamage(Damage);
        }

        private void PerformEnhancedAttack()
        {
            // OverlapSphere를 사용하여 범위 내 적 탐지
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, EnhancedRadius);

            foreach (Collider hitEnemy in hitEnemies)
            {
                if (hitEnemy.CompareTag("Enemy"))
                {
                    // 범위 내 적에게 데미지 전달
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
            // 에디터에서 강화 범위를 시각적으로 표시
            if (isEnhanced)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, EnhancedRadius);
            }
        }
    }
}
