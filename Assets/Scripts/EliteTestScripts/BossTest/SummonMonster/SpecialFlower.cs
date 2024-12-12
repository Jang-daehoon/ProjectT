using EnemyController;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFlower : EliteUnit
{
    private bool isCasting = false; // 현재 스킬 시전 중 여부
    private bool IsPlayerInRange = false; // 현재 스킬 시전 중 여부
    private bool shouldLook = true; // 플레이어를 바라볼 수 있는 상태인지

    public Collider attackRange; // 공격 범위
    public ParticleSystem grass; // 파티클 시스템

    private FlowerState currentState;

    private bool isAtkOn = false;
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 참조
        ChangeState(FlowerState.IDLE); // 초기 상태 설정
        StartCoroutine(LookTarget());
    }
    void Update()
    {
        HpBarUpdate();
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
        //test
        //if(Input.GetKeyDown(KeyCode.Escape))
        //    ChangeState(FlowerState.DIE);
    }
    private void ChangeState(FlowerState newState)
    {
        if (currentState == newState || currentState == FlowerState.DIE) return;

        Debug.Log($"State changed: {currentState} -> {newState}");
        currentState = newState;

        switch (newState)
        {
            case FlowerState.IDLE:
                StopAllCoroutines();
                animator.SetBool("isCasting", false); // 캐스팅 애니메이션 중지
                StartCoroutine(LookTarget());
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
        shouldLook = false; // 바라보기를 멈춤

        while (currentState == FlowerState.CAST)
        {
            yield return new WaitForSeconds(attackDelay); // 공격 딜레이
            if (IsPlayerInRange == false)
            {
                ChangeState(FlowerState.IDLE);
                yield break;
            }

            animator.SetBool("isCasting", true); // 캐스팅 애니메이션 시작
            yield return new WaitForSeconds(1f);
            grass.Play(); // 파티클 실행
            isAtkOn = true;

            yield return new WaitForSeconds(1f);
            animator.SetBool("isCasting", false);
            yield return new WaitForSeconds(attackDelay); // 공격 딜레이
        }

        shouldLook = true; // 바라보기를 재개
        isCasting = false;
    }
    private void AttackPlayer()
    {
        if (IsPlayerInRange == true)
        {
            Debug.Log("아야!");
            GameManager.Instance.player.TakeDamage(dmgValue);
        }
    }
    private IEnumerator LookTarget()
    {
        while (true)
        {
            if (shouldLook || !animator.GetBool("isCasting"))
            {
                // 바라보는 동작 수행
                Look(3f);
            }
            yield return null; // 매 프레임 대기
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        hpBar.HpBarUpdate();
        if (curHp <= 0)
        {
            ChangeState(FlowerState.DIE);
        }
    }
    private IEnumerator Die()
    {
        StopCoroutine(LookTarget());
        gameObject.GetComponent<Collider>().enabled = false;
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

    private void OnTriggerStay(Collider other)
    {
        if (isAtkOn == false) return;
        if (other.CompareTag("Player"))
        {
            AttackPlayer();
            isAtkOn = false;
        }
    }
}
