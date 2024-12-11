using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeafStorm : MonoBehaviour
{
    public ParticleSystem stormDuration; // ��ų ���� ��ƼŬ
    public ParticleSystem stormEnd; // ���� �� ��ƼŬ
    public float stormDurationTime = 5f; // ���� ���� �ð�
    public float moveSpeed = 6f;
    public float dmgValue = 20f;
    public Transform target; // �÷��̾� ��ġ

    private void Start()
    {
        StartCoroutine(ExecuteStorm());
    }
    private IEnumerator ExecuteStorm()
    {
        // ��ų ����: ���� ��ƼŬ ��� �� ���� ����
        stormDuration.Play();

        float timer = 0f;
        while (timer < stormDurationTime)
        {
            target = EliteBossGameMangerTest.Instance.player.transform;
            //target = GameManager.Instance.player.transform;
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
            timer += Time.deltaTime;
            yield return null;
        }

        stormDuration.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        stormEnd.Play();

        // ���� ��ƼŬ�� ���� ������ ���
        yield return new WaitForSeconds(stormEnd.main.duration);

        // ��ų ������Ʈ ��Ȱ��ȭ �Ǵ� ����
        Destroy(gameObject);
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO : ������ ���°� ����
            GameManager.Instance.player.TakeDamage(dmgValue * 0.5f);
        }
    }
}
