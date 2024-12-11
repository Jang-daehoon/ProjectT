using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f; // 투사체 속도
    public float lifetime = 5f; // 투사체가 존재하는 시간
    public float dmgValue = 20f;
    public ParticleSystem explosionEffect; // 터지는 파티클 효과
    public ParticleSystem trailEffect; // 이동 중 파티클 효과
    public LayerMask collisionMask; // 충돌할 레이어 (Player, Ground 등)

    private Transform target; // 플레이어를 타겟팅

    public void Initialize(Transform targetTransform)
    {
        target = targetTransform;
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Update()
    {
        if (target != null)
        {
            // 플레이어를 향해 이동
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // 투사체가 타겟을 바라보도록 회전
            transform.LookAt(target);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // 충돌 체크
        if ((collisionMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // 충돌 시 터지는 파티클 재생
            if (explosionEffect != null)
            {
                ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosion.Play();
                if (other.CompareTag("Player") == true)
                {
                    print($"{other.gameObject.name}");
                    GameManager.Instance.player.TakeDamage(dmgValue);
                }
                Destroy(explosion.gameObject, explosion.main.duration);
            }

            // 이동 중 파티클 제거
            if (trailEffect != null)
            {
                trailEffect.Stop();
                Destroy(trailEffect.gameObject, trailEffect.main.duration);
            }

            // 투사체 제거
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyAfterLifetime()
    {
        // 수명 끝난 후 투사체 제거
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
