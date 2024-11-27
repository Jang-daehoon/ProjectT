using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HoonsCodes
{
    public class BulletProjectile : MonoBehaviour
    {
        public float Damage;
        public float Speed;
        public float Lifetime = 3f; // 총알의 생명 시간 (초)
        public ParticleSystem HitParticle;

        private void Awake()
        {
            DestroyPrefabs();
        }

        // Update is called once per frame
        void Update()
        {
            // 총알을 전방으로 이동
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 충돌한 객체가 적인지 확인 
            // 적에게 데미지 전달 (Enemy 스크립트 필요)
            if (other.CompareTag("Enemy"))
            {
                ParticleSystem hitParticle = Instantiate(HitParticle, other.transform.position, transform.rotation);
                hitParticle.Play();
                other.GetComponent<ITakeDamage>().TakeDamage(Damage);
                Destroy(this);
            }
            // 충돌 시 총알 파괴
        }
        private void DestroyPrefabs()
        {
            Destroy(gameObject, Lifetime);
        }
    }

}
