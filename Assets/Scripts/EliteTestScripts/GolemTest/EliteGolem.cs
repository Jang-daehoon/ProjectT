using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System;
using UnityEditor;
using UnityEditor.ShaderGraph.Internal;
public class EliteGolem : EliteUnit
{
    [Tooltip("공격 범위")]
    public Collider attackRange;
    public Collider skillRange;

    public ParticleSystem skillParticle;
    public ParticleSystem attackParticleRight;
    public ParticleSystem attackParticleLeft;

    [Header("이동 관련 수치")]
    [SerializeField] private float lookSpeed = 3f; // 회전 속도

    [Header("스킬 관련 수치")]
    [SerializeField] private float skillCoolTime = 15.0f; // 스킬 쿨타임
    [SerializeField] private float skillGroggy = 4.0f;

    private AttackWarning attackWarning; // 경고 관리 스크립트

    private EliteState currentState;

    protected override void Awake()
    {
        base.Awake();
        attackWarning = GetComponent<AttackWarning>();

        agent.updateRotation = false;
        attackParticleRight.gameObject.SetActive(false);
        attackParticleLeft.gameObject.SetActive(false);
    }
    private void Start()
    {
        currentState = EliteState.RISE;
        StartCoroutine(GolemRise());
        StartCoroutine(UseSkill());
    }
    private void Update()
    {
        HpBarUpdate();
        target = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 참조

        switch (currentState)
        {
            case EliteState.IDLE:
                HandleIdleState();
                break;
            case EliteState.CHASE:
                HandleChaseState();
                break;
            case EliteState.ATTACK:
                break;
            case EliteState.SKILL:
                // 스킬은 코루틴으로 돌아감
                break;
            case EliteState.DIE:
                // 죽음 상태 처리
                break;
        }
    }
    private void ChangeState(EliteState newState)
    {
        Debug.Log($"[ChangeState] Called from: " +
            $"{new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}" +
            $" | Current: {currentState} -> New: {newState}");

        if (currentState == newState)
            return; // 상태가 이미 동일하면 실행하지 않음

        if (animator.GetBool("isSkillExecuting") == true && newState != EliteState.DIE)
            return;

        currentState = newState;
        switch (newState)
        {
            case EliteState.CHASE:
                if (Attacking()) return; // 공격 상태에서 추적 상태로 전환 제한
                agent.isStopped = false;
                animator.SetBool("isChasing", true);
                break;
            case EliteState.ATTACK:
                HandleAttackState();
                break;
            case EliteState.SKILL:
                StartCoroutine(HandleSkillState());
                break;
            case EliteState.DIE:
                StartCoroutine(HandleDieState());
                break;
        }
    }
    private IEnumerator GolemRise()
    {
        animator.SetTrigger("Rise");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Rise") == false)
            yield return null; // 애니메이션 시작 대기

        agent.isStopped = true;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 일어나는 애니메이션 종료
        attackParticleRight.gameObject.SetActive(true);
        attackParticleLeft.gameObject.SetActive(true);
        currentState = EliteState.IDLE;
    }
    private void HandleIdleState()
    {
        if (isPlayerInRange == true)
            ChangeState(EliteState.ATTACK);

        else if (target != null)
            ChangeState(EliteState.CHASE);
    }
    private void HandleChaseState()
    {
        if (isPlayerInRange == true)
            ChangeState(EliteState.ATTACK);
        else
            Move();
    }
    public override void Move()
    {
        // 스킬 또는 공격 상태 중 이동 금지
        if (currentState == EliteState.SKILL || currentState == EliteState.ATTACK || Attacking())
            return;

        EndAttackWarning();
        Look(lookSpeed);
        agent.isStopped = false;
        agent.SetDestination(target.position);
        animator.SetBool("isChasing", true);
    }
    private void HandleAttackState()
    {
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        animator.SetFloat("AttackSpeed", atkSpeed);
        animator.SetBool("isChasing", false);

        if (isPlayerInRange == false)
        {
            StartCoroutine(EndAttackWait(EliteState.IDLE));
            return;
        }
        // 공격 실행
        print("combo 실행됨");
        StartCoroutine(ComboAttack());
    }

    private IEnumerator ComboAttack()
    {
        animator.SetBool("isAttack", true);
        // 첫 번째 공격
        animator.SetTrigger("Attack1");

        if (isPlayerInRange == true)
            animator.SetTrigger("Attack2");
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(attackDelay);

        animator.SetBool("isAttack", false);
        ChangeState(EliteState.IDLE);
    }
    private IEnumerator EndAttackWait(EliteState nextState)
    {
        // 애니메이션 시작하는 딜레이 대기
        yield return new WaitForSeconds(0.1f);
        // 공격 애니메이션이 끝날 때까지 대기
        while (Attacking())
            yield return null;

        // NavMeshAgent 다시 활성화
        agent.isStopped = false;
        animator.SetBool("isAttack", false);

        // 상태 전환
        ChangeState(nextState);
    }
    private IEnumerator HandleSkillState()
    {
        if (animator.GetBool("isSkillExecuting") == true)
            yield break;

        animator.SetBool("isSkillExecuting", true);
        EndAttackWarning();
        animator.SetTrigger("Skill");
        agent.isStopped = true;
        yield return new WaitForSeconds(skillGroggy);
        animator.SetBool("isSkillExecuting", false);

        ChangeState(EliteState.IDLE);
    }
    private IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCoolTime);

            // 공격 중이면 대기 후 스킬 상태로 전환
            if (currentState == EliteState.ATTACK && Attacking())
                yield return new WaitUntil(() => !Attacking());

            if (currentState == EliteState.SKILL || currentState == EliteState.DIE)
                continue;

            ChangeState(EliteState.SKILL);
        }
    }
    private IEnumerator HandleDieState()
    {
        EndAttackWarning();
        agent.isStopped = true;
        agent.SetDestination(transform.position);
        animator.SetTrigger("Die");
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(1.7f);

        Destroy(gameObject);
    }

    public bool Attacking()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 공격 애니메이션 진행하는 동안 TRUE
        return (stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2")) && stateInfo.normalizedTime < 1f;
    }
    private void SkillParticle()
    {
        ParticleSystem HugeParticle = Instantiate(skillParticle, transform.position, transform.rotation);
        var main = HugeParticle.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }
    public void ApplyDamage()
    {
        // 공격 범위 안의 모든 대상 찾기
        Collider[] hitColliders = Physics.OverlapBox(attackRange.bounds.center,
            attackRange.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player") == true)
            {
                GameManager.Instance.player.TakeDamage(dmgValue);
                print("!!!!!!!!!!!!!!!!");
            }
        }
    }
    public void SkillApplyDamage()
    {
        float sphereRadius = skillRange.bounds.extents.x; // 스피어의 반경 설정
        Vector3 sphereCenter = skillRange.bounds.center; // 스피어의 중심 설정

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, LayerMask.GetMask("Player"));

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                GameManager.Instance.player.TakeDamage(dmgValue);
                Debug.Log("스킬로 플레이어에게 데미지 적용됨!");
            }
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (curHp <= 0)
        {
            ChangeState(EliteState.DIE);
        }
    }
    public void ShowAttackWarning()
    {
        if (currentState == EliteState.SKILL)
        {
            Quaternion baseRotation = transform.rotation;
            attackWarning.ShowWarning(transform.position, baseRotation, currentState);
        }
        else
        {
            Quaternion baseRotation = attackRange.transform.rotation; // BoxCollider의 회전값
            attackWarning.ShowWarning(attackRange.bounds.center, baseRotation, currentState);
        }
    }
    public void EndAttackWarning()
    {
        attackWarning.HideWarning();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") == true)
            isPlayerInRange = true;
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") == true)
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            isPlayerInRange = false;
        }
    }
}


