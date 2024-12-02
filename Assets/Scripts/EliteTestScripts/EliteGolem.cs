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
    public ParticleSystem attackParticleRight;
    public ParticleSystem attackParticleLeft;
    public NavMeshAgent agent { get; private set; }
    public Rigidbody rigidBody { get; private set; }
    public StateMachine stateMachine { get; private set; }

    [SerializeField] private float attackDelay = 1.5f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private bool isAttacking = false; // ���� �� ���� �÷���

    [SerializeField] private float skillCoolTime = 10.0f; // ��Ÿ�� (��)

    [SerializeField] private AttackWarning attackWarning; // ��� ���� ��ũ��Ʈ

    private EliteState currentState;
    private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� ����

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        attackParticleRight.gameObject.SetActive(false);
        attackParticleLeft.gameObject.SetActive(false);
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
                }
                else if
                    (target != null) currentState = EliteState.CHASE;
                break;
            case EliteState.CHASE:
                if (isPlayerInRange == true)
                    currentState = EliteState.ATTACK; // ���� �� ���� üũ
                else if (Attacking() == false) Move();
                break;
            case EliteState.ATTACK:
                agent.isStopped = true;
                break;
            case EliteState.SKILL:
                agent.isStopped = true;
                break;
            case EliteState.DIE:
                agent.isStopped = true;
                break;
        }
        // �� ��� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.Space))
            currentState = EliteState.DIE;
    }
    private IEnumerator GolemRise()
    {
        animator.SetTrigger("Rise");

        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Rise") == false)
            yield return null; // �ִϸ��̼� ���� ���

        float riseDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        agent.SetDestination(transform.position);
        agent.isStopped = true;

        yield return new WaitForSeconds(riseDuration);
        attackParticleRight.gameObject.SetActive(true);
        attackParticleLeft.gameObject.SetActive(true);
        currentState = EliteState.IDLE;
    }
    public IEnumerator Attack()
    {
        while (true)
        {
            if (isPlayerInRange == false) yield return null;

            else
            {
                animator.SetFloat("AttackSpeed", attackSpeed);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttack", true);

                if (isPlayerInRange == true)
                {
                    if (animator.GetBool("comboAttack") == true)
                    {
                        animator.SetBool("isAttack", false);
                        yield return new WaitForSeconds(attackDelay);
                        currentState = EliteState.IDLE;
                    }

                    yield return new WaitForSeconds(attackDelay);

                    if (animator.GetBool("comboAttack") == false)
                        animator.SetBool("comboAttack", true);
                }
                else
                    currentState = EliteState.IDLE;
            }
        }
    }
    // ���� �ִϸ��̼� ������ 
    public bool Attacking()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if ((stateInfo.IsName("Attack1") || stateInfo.IsName("Attack2")) && stateInfo.normalizedTime < 1f)
            isAttacking = true; // �ִϸ��̼� ���� ��
        else
            isAttacking = false; // �ִϸ��̼� ����

        return isAttacking;
    }
    private IEnumerator UseSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(skillCoolTime);
            currentState = EliteState.SKILL;
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
        // ������ �����ϸ� ���� �ʱ�ȭ
        EndAttackWarning();
        animator.SetBool("isAttack", false);
        animator.SetBool("comboAttack", false);

        agent.isStopped = false;
        animator.SetBool("isChasing", true);
        agent.SetDestination(target.position);
    }

    public void ApplyDamage()
    {
        // ���� ���� ���� ��� ��� ã��
        Collider[] hitColliders = Physics.OverlapBox(attackRange.bounds.center,
            attackRange.bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));

        foreach (Collider hit in hitColliders)
        {
            //PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
            //    playerHealth.TakeDamage(attackDamage); // attackDamage�� Golem�� ���ݷ�
            //    Debug.Log("Player���� ������ ����!");
            //}
        }
    }
    public override void Dead()
    {
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
            Quaternion baseRotation = attackRange.transform.rotation; // BoxCollider�� ȸ����
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


