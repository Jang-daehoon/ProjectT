using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public float speed = 10f; // ����ü �ӵ�
    public float lifetime = 5f; // ����ü�� �����ϴ� �ð�
    public ParticleSystem explosionEffect; // ������ ��ƼŬ ȿ��
    public ParticleSystem trailEffect; // �̵� �� ��ƼŬ ȿ��
    public LayerMask collisionMask; // �浹�� ���̾� (Player, Ground ��)

    private Transform target; // �÷��̾ Ÿ����

    public void Initialize(Transform targetTransform)
    {
        target = targetTransform;
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Update()
    {
        if (target != null)
        {
            // �÷��̾ ���� �̵�
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // ����ü�� Ÿ���� �ٶ󺸵��� ȸ��
            transform.LookAt(target);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // �浹 üũ
        if ((collisionMask.value & (1 << other.gameObject.layer)) != 0)
        {
            // �浹 �� ������ ��ƼŬ ���
            if (explosionEffect != null)
            {
                ParticleSystem explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                explosion.Play();
                print($"{other.gameObject.name}");
                Destroy(explosion.gameObject, explosion.main.duration);
            }

            // �̵� �� ��ƼŬ ����
            if (trailEffect != null)
            {
                trailEffect.Stop();
                Destroy(trailEffect.gameObject, trailEffect.main.duration);
            }

            // ����ü ����
            Destroy(gameObject);
        }
    }
    private IEnumerator DestroyAfterLifetime()
    {
        // ���� ���� �� ����ü ����
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
