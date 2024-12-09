using EnemyController;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFlower : Character
{
    private Transform target; // 플레이어 참조

    public float attackInterval = 2.0f; // 공격 간 딜레이
    private bool isCasting = false; // 현재 스킬 시전 중 여부
    private bool IsPlayerInRange = false; // 현재 스킬 시전 중 여부
    public Collider attackRange; // 공격 범위
    public ParticleSystem grass; // 파티클 시스템
    private FlowerState currentState;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 참조
        ChangeState(FlowerState.IDLE); // 초기 상태 설정
        StartCoroutine(LookTarget());
    }

    void Update()
    {
        // 상태별 동작
        switch (currentState)
        {
            case FlowerState.IDLE:
                HandleIdleState();
                break;

            case FlowerState.CAST:
                HandleCastState();
                break;

            case FlowerState.DIE:
                break;
        }
    }

    private void ChangeState(FlowerState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"State changed: {currentState} -> {newState}");
        currentState = newState;

        switch (newState)
        {
            case FlowerState.IDLE:
                StopAllCoroutines();
                animator.SetBool("isCasting", false); // 캐스팅 애니메이션 중지
                break;

            case FlowerState.CAST:
                StartCoroutine(CastSkill()); // 스킬 시전 시작
                break;

            case FlowerState.DIE:
                animator.SetTrigger("Die");
                StartCoroutine(Die());
                break;
        }
    }

    private void HandleIdleState()
    {
        if (IsPlayerInRange == true)
            ChangeState(FlowerState.CAST);
    }

    private void HandleCastState()
    {
        if (IsPlayerInRange == false)
            ChangeState(FlowerState.IDLE);
    }

    private IEnumerator CastSkill()
    {
        isCasting = true;

        while (currentState == FlowerState.CAST)
        {
            yield return new WaitForSeconds(attackInterval); // 공격 딜레이
            if (IsPlayerInRange == false)
            {
                ChangeState(FlowerState.IDLE);
                yield break;
            }

            animator.SetBool("isCasting", true); // 캐스팅 애니메이션 시작
            yield return new WaitForSeconds(1f);
            grass.Play(); // 파티클 실행
            AttackPlayer();

            yield return null;
            animator.SetBool("isCasting", false);
            yield return new WaitForSeconds(attackInterval); // 공격 딜레이
        }

        isCasting = false;
    }

    private void AttackPlayer()
    {
        if (IsPlayerInRange == true)
        {
            Debug.Log("아야!");

        }
    }
    private IEnumerator LookTarget()
    {
        while (true)
        {
            if (animator.GetBool("isCasting") == true)
                yield return new WaitForSeconds(2.5f);
            Move();
            yield return null;
        }
    }
    public override void Move()
    {
        if (target == null) return; // 타겟이 없으면 리턴

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y축 회전을 고정

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(2.0f); // 죽음 애니메이션 대기
        BossDryad boss = FindObjectOfType<BossDryad>();
        if (boss != null)
            boss.RemoveMonster(gameObject);
        Destroy(gameObject);
    }


    public override void Dead()
    {
        ChangeState(FlowerState.DIE);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = true; // 플레이어가 범위에 들어옴
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = false; // 플레이어가 범위를 벗어남
        }
    }
}
