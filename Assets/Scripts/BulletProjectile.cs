using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoonsCodes
{
    public class BulletProjectile : MonoBehaviour
    {
        public float Damage;
        public float Speed;
        public float Lifetime = 3f; // �Ѿ��� ���� �ð� (��)
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
            // �浹�� ��ü�� ������ Ȯ�� 
            // ������ ������ ���� (Enemy ��ũ��Ʈ �ʿ�)
            if (other.CompareTag("Enemy"))
            {
                ParticleSystem hitParticle = Instantiate(HitParticle, other.transform.position, transform.rotation);
                hitParticle.Play();
                other.GetComponent<ITakeDamage>().TakeDamage(Damage);
                Destroy(this);
            }
            // �浹 �� �Ѿ� �ı�
        }
        private void DestroyPrefabs()
        {
            Destroy(gameObject, Lifetime);
        }
    }

}
