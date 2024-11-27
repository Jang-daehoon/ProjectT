using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System;

public class EliteGolem : Character
{
    public Transform target;
    public BoxCollider attackRange;
    public ParticleSystem skillParticle;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }
    [SerializeField] private float curAnimationTime;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float lastSkillTime = 10f; // ������ ��ų ��� �ð� (�ʱⰪ�� ��Ÿ���� �ٷ� ����� �� �ְ� ����)
    [SerializeField] private float skillCoolTime = 10.0f; // ��Ÿ�� (��)
    [SerializeField] private bool isSkillOnCooldown = false;
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
    private void Start()
    {
        StartCoroutine(UseSkill());
    }
    private void Update()
    {
        Move();
    }
    public IEnumerator Attack()
    {
        Debug.Log($"Attack ����, comboAttack: {animator.GetInteger("comboAttack")}");
        agent.SetDestination(transform.position); // ���� ��ġ ����
        agent.isStopped = true;
        while (animator.GetBool("isAttack") == true) // ���� ���°� �����Ǵ� ����
        {
            // ���� ���ݰ� ������ ����
            yield return new WaitForSeconds(attackDelay);

            // �ִϸ��̼� ���� ����
            if (animator.GetInteger("comboAttack") == 0)
                animator.SetInteger("comboAttack", 1);
            else
                animator.SetInteger("comboAttack", 0);
        }

        // ������ ���� �� �̵� �����ϵ��� ����
        Debug.Log("Attack ����");
        agent.isStopped = false;
    }
    private IEnumerator UseSkill()
    {
        yield return new WaitForSeconds(skillCoolTime);

        while (true)
        {
            Debug.Log("��ų �ߵ�!");
            animator.SetTrigger("Skill");
            agent.SetDestination(transform.position); // ���� ��ġ ����
            yield return new WaitForSeconds(skillCoolTime);
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
        if (animator.GetBool("isAttack") == false)
        {
            currentState = State.CHASE;
            animator.SetBool("isChasing", true);
            agent.SetDestination(target.position);
        }
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
