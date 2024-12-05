using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;

public class BossDryad : Character
{
    public Transform target;
    [Tooltip("�Ϲ� ���� �Ÿ�")]
    public float attackDistance = 25f;
    public NavMeshAgent agent { get; private set; }
    [SerializeField] public LeafStorm leafStormInstance;
    [SerializeField] private float attackDelay = 1.5f; // ���ݰ� ������

    [SerializeField] private float skillGroggy = 3.5f;
    [SerializeField] private float LeafStormCoolTime = 15f;

    private BossState currentState;
    [SerializeField] private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� ����
    [SerializeField] private bool isSkillExecuting = false; // ��ų ���� ���� ���� �÷���

    public ParticleSystem LeafStormParticle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
    }
    private void Start()
    {
        StartCoroutine(Attack());
        StartCoroutine(UseSkills());
        currentState = BossState.IDLE;
    }
    private void Update()
    {
        target = EliteBossGameMangerTest.Instance.player.transform;
        //target = GameManager.Instance.player.transform;
        OnRangeAttack();
        Look(3f);

        switch (currentState)
        {
            case BossState.IDLE:
                HandleIdleState();
                break;
            case BossState.CHASE:
                HandleChaseState();
                break;
            case BossState.ATTACK:
                HandleAttackState();
                break;
            case BossState.LEAFSTORM:
            case BossState.SKILL2:
            case BossState.SKILL3:
                // ��ų�� �ڷ�ƾ���� ó��
                break;
            case BossState.DIE:
                // ���� ���� ó��
                break;
        }
    }
    private void ChangeState(BossState newState)
    {
        Debug.Log($"[ChangeState] Called from: " +
            $"{new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}" +
            $" | Current: {currentState} -> New: {newState}");

        if (currentState == newState || isSkillExecuting)
            return;

        currentState = newState;

        switch (newState)
        {
            case BossState.CHASE:
                agent.isStopped = false;
                animator.SetBool("isChasing", true);
                break;
            case BossState.ATTACK:
                agent.isStopped = true;
                break;
            case BossState.LEAFSTORM:
                StartCoroutine(HandleSkillState(leafStormInstance));
                break;
            case BossState.DIE:
                StartCoroutine(HandleDieState());
                break;
        }
    }
    private void HandleIdleState()
    {
        if (isPlayerInRange)
            ChangeState(BossState.ATTACK);
        else if (target != null)
            ChangeState(BossState.CHASE);
    }
    private void HandleChaseState()
    {
        if (isPlayerInRange)
            ChangeState(BossState.ATTACK);
        else
            Move();
    }
    private void HandleAttackState()
    {
        if (!isPlayerInRange)
            ChangeState(BossState.IDLE); // �÷��̾ �������� ����� IDLE ���·� ��ȯ
    }

    private IEnumerator HandleSkillState(LeafStorm skill)
    {
        if (isSkillExecuting)
            yield break;

        isSkillExecuting = true;
        currentState = BossState.LEAFSTORM; // ��ų ���¸� ��������� ����
        animator.SetTrigger($"{skill.name}");
        agent.isStopped = true;
        LeafStormParticle.Play();
        yield return new WaitForSeconds(skillGroggy);

        isSkillExecuting = false;
        ChangeState(BossState.IDLE);
    }
    private IEnumerator UseSkills()
    {
        while (true)
        {
            yield return new WaitForSeconds(LeafStormCoolTime);
            if (currentState != BossState.DIE)
                ChangeState(BossState.LEAFSTORM);
        }
    }
    public void LeafStormEvent()
    {
        Instantiate(leafStormInstance, transform.position, transform.rotation);
    }
    private IEnumerator HandleDieState()
    {
        agent.isStopped = true;
        animator.SetTrigger("Die");
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
    public override void Dead()
    {
    }
    public override void Move()
    {
        if (currentState == BossState.LEAFSTORM || currentState == BossState.SKILL2 || currentState == BossState.SKILL3)
            return;
        if (currentState == BossState.ATTACK) return;
        agent.SetDestination(target.position);
    }
    private IEnumerator Attack()
    {
        while (true)
        {
            // �÷��̾ ������ ������ ���
            if (isPlayerInRange == false)
            {
                animator.SetBool("isAttack", false); // ���� �ִϸ��̼� ��Ȱ��ȭ
                yield return null;
            }
            else
            {
                animator.SetBool("isChasing", false);
                Look(1.5f); // �÷��̾ �ٶ�
                animator.SetBool("isAttack", true); // ���� �ִϸ��̼� Ȱ��ȭ

                yield return new WaitForSeconds(0.1f); // �ִϸ��̼� ��� �ð���ŭ ���

                // ���� ������
                animator.SetBool("isAttack", false); // ���� �ִϸ��̼� ��Ȱ��ȭ
                yield return new WaitForSeconds(attackDelay); // ���� �� ������
            }
        }
    }
    public void OnRangeAttack()
    {
        float distance = Vector3.Distance(
            new Vector3(target.position.x, 0, target.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));
        if (attackDistance >= distance)
            isPlayerInRange = true;
        else
            isPlayerInRange = false;
    }
    public void Look(float lookSpeed)
    {
        if (target == null) return; // Ÿ���� ������ ����

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y�� ȸ���� ����

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }
}
