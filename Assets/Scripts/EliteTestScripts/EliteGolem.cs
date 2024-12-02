using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System;
using UnityEditor;
public class EliteGolem : Character
{
    public Transform target;
    [Tooltip("공격 범위")]
    public Collider attackRange;
    public ParticleSystem skillParticle;
    public ParticleSystem attackParticleRight;
    public ParticleSystem attackParticleLeft;
    public NavMeshAgent agent { get; private set; }

    [Tooltip("공격")]
    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float attackSpeed = 1f;

    [Tooltip("스킬")]
    [SerializeField] private float skillCoolTime = 15.0f; // 스킬 쿨타임
    [SerializeField] private float skillGroggy = 13.0f; // 스킬 쿨타임

    [SerializeField] private AttackWarning attackWarning; // 경고 관리 스크립트

    private EliteState currentState;
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        attackParticleRight.gameObject.SetActive(false);
        attackParticleLeft.gameObject.SetActive(false);
    }
    private void Start()
    {
        currentState = EliteState.RISE;
        StartCoroutine(GolemRise());
        StartCoroutine(UseSkill());
    }
    private void OnEnable()
    {
    }

    private void Update()
    {
        switch (currentState)
        {
            case EliteState.IDLE:
                HandleIdleState();
                break;
            case EliteState.CHASE:
                HandleChaseState();
                break;
            case EliteState.ATTACK:
                HandleAttackState();
                break;
            case EliteState.SKILL:
                // 스킬은 코루틴으로 돌아감
                break;
            case EliteState.DIE:
                // 죽음 상태 처리
                break;
        }
        // 골렘 사망 테스트
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeState(EliteState.DIE);
        print($"현재 상태 : {currentState}, 범위 안? : {isPlayerInRange}");
    }
    private void ChangeState(EliteState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case EliteState.CHASE:
                if (Attacking()) return; // 공격 상태에서 추적 상태로 전환 제한
                agent.isStopped = false;
                animator.SetBool("isChasing", true);
                break;
            case EliteState.ATTACK:
                agent.isStopped = true;
                animator.SetBool("isAttack", true);
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
        attackParticleRight.gameObject.SetActive(true);
        attackParticleLeft.gameObject.SetActive(true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // 일어나는 애니메이션 종료
        currentState = EliteState.IDLE;
    }
    private void HandleIdleState()
    {
        if (isPlayerInRange)
            ChangeState(EliteState.ATTACK);
        else if (target != null)
            ChangeState(EliteState.CHASE);
    }
    private void HandleChaseState()
    {
        if (isPlayerInRange)
            ChangeState(EliteState.ATTACK);
        else
            Move();
    }
    public override void Move()
    {
        if (Attacking())
            return;
        EndAttackWarning();
        agent.isStopped = false;
        agent.SetDestination(target.position);
        animator.SetBool("isChasing", true);
    }
    private IEnumerator DelayStateChange(EliteState nextState, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeState(nextState);
    }
    private void HandleAttackState()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        animator.SetFloat("AttackSpeed", attackSpeed);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttack", true);

        if (Attacking())
            return; // 공격 중이면 상태 전환을 제한
        if (!isPlayerInRange)
        {
            StartCoroutine(EndAttackWait(EliteState.IDLE));
            return;
        }
        // 공격 실행
        StartCoroutine(PerformComboAttack());
    }

    private IEnumerator PerformComboAttack()
    {
        // 첫 번째 공격
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(attackDelay);

        // 플레이어가 여전히 범위 안에 있으면 두 번째 공격
        if (isPlayerInRange)
        {
            animator.SetTrigger("Attack2");
            yield return new WaitForSeconds(attackDelay);
        }

        // 공격 후 잠깐 쉬고 IDLE 전환
        yield return new WaitForSeconds(attackDelay * 2f);
        ChangeState(EliteState.IDLE);
    }
    private IEnumerator EndAttackWait(EliteState nextState)
    {
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
        // 스킬 애니메이션 실행
        EndAttackWarning();
        animator.SetTrigger("Skill");
        agent.isStopped = true;

        // 스킬 이펙트 생성은 이벤트로
        yield return new WaitForSeconds(skillGroggy);

        // 스킬 종료 후 Idle 상태로 전환
        ChangeState(EliteState.IDLE);
    }
    private IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCoolTime); // 15초 대기

            // 스킬 발동 가능한 상태인지 확인
            if (currentState != EliteState.SKILL)
            {
                ChangeState(EliteState.SKILL);
            }
        }
    }

    private IEnumerator HandleDieState()
    {
        agent.isStopped = true;
        animator.SetTrigger("Die");
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

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
            //PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(attackDamage); // attackDamage는 Golem의 공격력
            //    Debug.Log("Player에게 데미지 적용!");
            //}
        }
    }
    public override void Dead()
    {
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            isPlayerInRange = true;
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.ResetTrigger("Attack1");
            animator.ResetTrigger("Attack2");
            isPlayerInRange = false;
        }
    }
}


