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
        Debug.Log($"Attack ����, comboAttack: {animator.GetInteger("comboAttack")}");
        // �̵� ���߱�
        agent.isStopped = true;
        agent.SetDestination(transform.position); // ���� ��ġ ����
        while (animator.GetBool("isAttack") == true) // ���� ���°� �����Ǵ� ����
        {
            // �ִϸ��̼� ���� ����
            if (animator.GetInteger("comboAttack") == 0)
                animator.SetInteger("comboAttack", 1);
            else
                animator.SetInteger("comboAttack", 0);

            // ���� ���ݰ� ������ ����
            yield return new WaitForSeconds(attackDelay);
        }

        // ������ ���� �� �̵� �����ϵ��� ����
        Debug.Log("Attack ����");
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
            print("���ݹ��� ����");
            StartCoroutine(Attack());
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            animator.SetBool("isAttack", false);
            animator.SetInteger("comboAttack", 0);
            print("���ݹ��� ����");
            StopCoroutine(Attack()); // ���� �ڷ�ƾ ����
        }
    }
}
