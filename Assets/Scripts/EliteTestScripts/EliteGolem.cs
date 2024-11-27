using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using System;

public class EliteGolem : Character
{
    public Transform target;
    public BoxCollider attackRange;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }
    [SerializeField] private float curAnimationTime;
    [SerializeField] private float attackDelay = 2.0f;
    [SerializeField] private float attackSpeed = 1f;
    private enum State
    {
        IDLE,
        CHASE,
        ATTACK,
        KILLED
    }

    private State currentState = State.IDLE;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        Move();
    }
    public IEnumerator Attack()
    {
        Debug.Log($"Attack 시작, comboAttack: {animator.GetInteger("comboAttack")}");
        // 이동 멈추기
        agent.isStopped = true;
        agent.SetDestination(transform.position); // 현재 위치 고정
        while (animator.GetBool("isAttack") == true) // 공격 상태가 유지되는 동안
        {
            // 애니메이션 상태 갱신
            if (animator.GetInteger("comboAttack") == 0)
                animator.SetInteger("comboAttack", 1);
            else
                animator.SetInteger("comboAttack", 0);

            // 연속 공격간 딜레이 설정
            yield return new WaitForSeconds(attackDelay);
        }

        // 공격이 끝난 후 이동 가능하도록 설정
        Debug.Log("Attack 종료");
        agent.isStopped = false;
    }
    public override void Move()
    {
        if (animator.GetBool("isAttack") == false)
        {
            currentState = State.CHASE;
            animator.SetBool("isChasing", true);
            agent.SetDestination(target.position);
        }
    }

    public override void TakeDamage(float damage)
    {
    }

    public override void Dead()
    {
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            currentState = State.ATTACK;
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetBool("isChasing", false);
            animator.SetBool("isAttack", true);
            print("공격범위 들어옴");
            StartCoroutine(Attack());
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("isAttack", false);
            animator.SetInteger("comboAttack", 0);
            print("공격범위 나감");
            StopCoroutine(Attack()); // 공격 코루틴 중지
        }
    }
}
