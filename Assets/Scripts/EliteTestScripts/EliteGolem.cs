using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System;
using UnityEngine.Timeline;

public class EliteGolem : Character
{
    public Transform target;
    public BoxCollider attackRange;
    public ParticleSystem skillParticle;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }

    [SerializeField] private float curAnimationTime;
    [SerializeField] private float attackDelay = 2.7f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private bool isAttacking = false; // 공격 중 여부 플래그
    [SerializeField] private bool attackDontMove = false;

    [SerializeField] private float skillCoolTime = 10.0f; // 쿨타임 (초)

    [SerializeField] private AttackWarning attackWarning; // 경고 관리 스크립트

    private State currentState;
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부

    private enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        SKILL,
        DIE
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentState = State.IDLE;
    }
    private void Start()
    {
        StartCoroutine(UseSkill());
    }
    private void Update()
    {
        switch (currentState)
        {
            case State.IDLE:
                if (isPlayerInRange) currentState = State.ATTACK;
                if (target != null) currentState = State.CHASE;
                break;
            case State.CHASE:
                animator.SetBool("isAttack", false);
                animator.SetInteger("comboAttack", 0);
                if (isPlayerInRange) currentState = State.ATTACK; // 범위 내 진입 체크
                if (Attacking() == false) Move();
                break;
            case State.ATTACK:
                if (attackDontMove == false) StartCoroutine(Attack());
                if (isPlayerInRange == false) currentState = State.CHASE; // 범위 나감
                break;
            case State.SKILL:
                agent.isStopped = true;
                break;
            case State.DIE:
                animator.SetTrigger("Die");
                break;
        }
    }
    public IEnumerator Attack()
    {
        if (attackDontMove) yield break;

        Debug.Log($"Attack 시작, comboAttack: {animator.GetInteger("comboAttack")}");
        attackDontMove = true;

        animator.SetFloat("AttackSpeed", attackSpeed);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttack", true);

        while (animator.GetBool("isAttack") == true) // 공격 상태가 유지되는 동안
        {
            // 현재 위치 고정
            agent.SetDestination(transform.position);
            agent.isStopped = true;

            // 연속 공격간 딜레이 설정
            yield return new WaitForSeconds(attackDelay);

            // 애니메이션 상태 갱신
            if (animator.GetInteger("comboAttack") == 0)
                animator.SetInteger("comboAttack", 1);
            else
                animator.SetInteger("comboAttack", 0);

            float delay = animator.GetCurrentAnimatorStateInfo(0).length;
            attackDelay = delay;
        }

        attackDontMove = false;
        Debug.Log("Attack 종료");
    }
    public bool Attacking()
    {
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Skill");
        return isAttacking;
    }
    private IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCoolTime);
            Debug.Log("스킬 발동!");
            currentState = State.SKILL;

            agent.SetDestination(transform.position); // 현재 위치 고정
            animator.SetTrigger("Skill");

            // 스킬 지속 시간 대기
            yield return new WaitForSeconds(4f); // 스킬 애니메이션이 끝나는 시간

            // 스킬 종료 후 다시 추적 상태로 전환
            currentState = State.IDLE;
            agent.isStopped = true;
        }
    }
    private void SkillParticle()
    {
        ParticleSystem HugeParticle = Instantiate(skillParticle, transform.position, transform.rotation);
        var main = HugeParticle.main;
        main.loop = false;
        main.stopAction = ParticleSystemStopAction.Destroy;
    }

    public override void Move()
    {
        agent.isStopped = false;
        animator.SetBool("isChasing", true);
        agent.SetDestination(target.position);
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
        if (attackWarning != null) attackWarning.ShowWarning(attackRange.bounds.center);
        else attackWarning.HideWarning();
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInRange = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            isPlayerInRange = true;
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))

            isPlayerInRange = false;
    }
}


