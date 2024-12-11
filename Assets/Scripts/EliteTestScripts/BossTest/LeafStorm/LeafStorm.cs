using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LeafStorm : MonoBehaviour
{
    public ParticleSystem stormDuration; // 스킬 지속 파티클
    public ParticleSystem stormEnd; // 종료 시 파티클
    public float stormDurationTime = 5f; // 추적 지속 시간
    public float moveSpeed = 6f;
    public float dmgValue = 20f;
    public Transform target; // 플레이어 위치

    private void Start()
    {
        StartCoroutine(ExecuteStorm());
    }
    private IEnumerator ExecuteStorm()
    {
        // 스킬 시작: 지속 파티클 재생 및 추적 시작
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

        // 종료 파티클이 끝날 때까지 대기
        yield return new WaitForSeconds(stormEnd.main.duration);

        // 스킬 오브젝트 비활성화 또는 제거
        Destroy(gameObject);
    }
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO : 데미지 들어가는거 구현
            GameManager.Instance.player.TakeDamage(dmgValue * 0.5f);
        }
    }
}
