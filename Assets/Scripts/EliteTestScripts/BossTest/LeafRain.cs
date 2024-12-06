using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafRain : MonoBehaviour
{
    public float damageRadius = 3f; // ������ ����
    public LayerMask damageableLayer; // �������� �� ���̾�
    public int damageAmount = 50; // ������ ��
    public float colliderActivationDelay = 1.5f; // �ݶ��̴� Ȱ��ȭ ������
    public float lifeTime = 2f; // LeafRain ���� �ð�
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
        Destroy(gameObject, lifeTime); // ���� �ð� ���� �ڵ� ����
    }

    private IEnumerator ActivateCollider()
    {
        // �ݶ��̴� Ȱ��ȭ ���
        rainParticle.Play();
        yield return new WaitForSeconds(colliderActivationDelay);
        coll.enabled = true;

        // OverlapSphere�� ������ ó��
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
