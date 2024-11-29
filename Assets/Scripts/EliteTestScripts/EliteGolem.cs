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
    public BoxCollider attackRange;
    public ParticleSystem skillParticle;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }

    [SerializeField] private float attackDelay = 2.5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private bool isAttacking = false; // 공격 중 여부 플래그

    [SerializeField] private float skillCoolTime = 10.0f; // 쿨타임 (초)

    [SerializeField] private AttackWarning attackWarning; // 경고 관리 스크립트

    private EliteState currentState;
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentState = EliteState.RISE;
    }
    private void Start()
    {
        StartCoroutine(GolemRise());
        StartCoroutine(Attack());
        StartCoroutine(UseSkill());
        StartCoroutine(DeadMotion());
    }

    private void Update()
    {
        switch (currentState)
        {
            case EliteState.IDLE:
                if (isPlayerInRange == true)
                {
                    currentState = EliteState.ATTACK;
                    break;
                }
                else if (target != null) currentState = EliteState.CHASE;
                break;
            case EliteState.CHASE:
                if (isPlayerInRange == true)
                {
                    currentState = EliteState.ATTACK; // 범위 내 진입 체크
                    break;
                }
                else if (Attacking() == false) Move();
                break;
            case EliteState.ATTACK:
                if (isPlayerInRange == false) currentState = EliteState.IDLE; // 범위 나감
                break;
            case EliteState.SKILL:
                agent.isStopped = true;
                break;
            case EliteState.DIE:
                agent.isStopped = true;
                break;
        }
        // 골렘 사망 테스트
        if (Input.GetKeyDown(KeyCode.Space))
            currentState = EliteState.DIE;
    }
    private IEnumerator GolemRise()
    {
        animator.SetTrigger("Rise");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Rise") == false)
            yield return null; // 애니메이션 시작 대기

        float riseDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        agent.SetDestination(transform.position);
        agent.isStopped = true;

        yield return new WaitForSeconds(riseDuration);
        currentState = EliteState.IDLE;
    }
    public IEnumerator Attack()
    {
        while (true)
        {
            if (currentState != EliteState.ATTACK) yield return null;

            else
            {
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttack", true);

                while (isPlayerInRange == true) // 공격 범위에 있는 동안
                {
                    // 현재 위치 고정
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;

                    if (animator.GetInteger("comboAttack") == 0)
                        animator.SetInteger("comboAttack", 1);
                    else
                        animator.SetInteger("comboAttack", 0);

                    // 연속 공격간 딜레이 설정
                    yield return new WaitForSeconds(attackDelay);
                }
            }
        }
    }
    // 공격 애니메이션 진행중 
    public bool Attacking()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if ((stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2")) && stateInfo.normalizedTime < 1f)
            isAttacking = true; // 애니메이션 진행 중
        else
            isAttacking = false; // 애니메이션 종료

        return isAttacking;
    }
    private IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCoolTime);
            currentState = EliteState.SKILL;

            agent.isStopped = true;
            agent.SetDestination(transform.position);   
            animator.SetTrigger("Skill");

            yield return new WaitForSeconds(4f);
            currentState = EliteState.IDLE;
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
        // 움직임 시작하면 공격 초기화
        EndAttackWarning();
        animator.SetBool("isAttack", false);
        animator.SetInteger("comboAttack", 0);

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
        //animator.SetTrigger("Die");
    }
    private IEnumerator DeadMotion()
    {
        while (true)
        {
            if (currentState == EliteState.DIE)
            {
                EndAttackWarning(); 
                animator.SetTrigger("Die");
                gameObject.GetComponent<Collider>().enabled = false;
                yield return new WaitForSeconds(1.7f);
                Destroy(gameObject);
            }
            else
                yield return null;
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


