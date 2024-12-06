using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRain : MonoBehaviour
{
    public float damageRadius = 3f; // 데미지 범위
    public LayerMask damageableLayer; // 데미지를 줄 레이어
    public int damageAmount = 50; // 데미지 양
    public float colliderActivationDelay = 1.5f; // 콜라이더 활성화 딜레이
    public float lifeTime = 2f; // LeafRain 지속 시간
    private Collider coll;

    public ParticleSystem rainParticle;
    private void Awake()
    {
        coll = GetComponentInChildren<Collider>();
        coll.enabled = false;
    }
    private void Start()
    {
        StartCoroutine(ActivateCollider());
        Destroy(gameObject, lifeTime); // 지속 시간 이후 자동 제거
    }

    private IEnumerator ActivateCollider()
    {
        // 콜라이더 활성화 대기
        rainParticle.Play();
        yield return new WaitForSeconds(colliderActivationDelay);
        coll.enabled = true;

        // OverlapSphere로 데미지 처리
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius, damageableLayer);
        foreach (Collider collider in hitColliders)
        {
            //if (collider.TryGetComponent(out Character target))
            //{
            //    target.TakeDamage(damageAmount);
            //}
        }
    }
}
