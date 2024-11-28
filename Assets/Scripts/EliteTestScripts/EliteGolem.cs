using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
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
    [SerializeField] private bool isAttacking = false; // ���� �� ���� �÷���
    [SerializeField] private bool attackDontMove = false;

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

        currentState = EliteState.IDLE;
    }
    private void Start()
    {
        StartCoroutine(UseSkill());
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
                if (target != null) currentState = EliteState.CHASE;
                break;
            case EliteState.CHASE:
                animator.SetBool("isAttack", false);
                animator.SetInteger("comboAttack", 0);
                if (isPlayerInRange == true) currentState = EliteState.ATTACK; // ���� �� ���� üũ
                if (Attacking() == false) Move();
                break;
            case EliteState.ATTACK:
                if (attackDontMove == false) StartCoroutine(Attack());
                if (isPlayerInRange == false) currentState = EliteState.CHASE; // ���� ����
                break;
            case EliteState.SKILL:
                agent.isStopped = true;
                break;
            case EliteState.DIE:
                animator.SetTrigger("Die");
                break;
        }
    }
    public IEnumerator Attack()
    {
        if (attackDontMove) yield break;

        Debug.Log($"Attack ����, comboAttack: {animator.GetInteger("comboAttack")}");
        attackDontMove = true;

        animator.SetFloat("AttackSpeed", attackSpeed);
        animator.SetBool("isChasing", false);
        animator.SetBool("isAttack", true);

        while (animator.GetBool("isAttack") == true) // ���� ���°� �����Ǵ� ����
        {
            // ���� ��ġ ����
            agent.SetDestination(transform.position);
            agent.isStopped = true;

            // ���� ���ݰ� ������ ����
            yield return new WaitForSeconds(attackDelay);

            // �ִϸ��̼� ���� ����
            if (animator.GetInteger("comboAttack") == 0)
                animator.SetInteger("comboAttack", 1);
            else
                animator.SetInteger("comboAttack", 0);

            float delay = animator.GetCurrentAnimatorStateInfo(0).length;
            attackDelay = delay;
        }

        attackDontMove = false;
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
            Debug.Log("��ų �ߵ�!");
            currentState = EliteState.SKILL;

            // ��ų ���� �� NavMeshAgent ����
            agent.isStopped = true;
            agent.SetDestination(transform.position);   
            animator.SetTrigger("Skill");

            // ��ų ���� �ð� ���
            yield return new WaitForSeconds(4f); // ��ų �ִϸ��̼��� ������ �ð�
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
    public void ShowAttackWarning()
    {
        if (currentState == EliteState.ATTACK)
        {
            Quaternion baseRotation = attackRange.transform.rotation; // BoxCollider�� ȸ����
            attackWarning.ShowWarning(attackRange.bounds.center, baseRotation, currentState);
        }
        else if (currentState == EliteState.SKILL)
        {
            Quaternion baseRotation = transform.rotation;
            attackWarning.ShowWarning(transform.position, baseRotation, currentState);
        }
    }
    public void EndAttackWarning()
    {
        attackWarning.HideWarning(currentState);
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


