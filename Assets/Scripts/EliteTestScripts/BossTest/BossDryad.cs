using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System.IO;

public class BossDryad : EliteUnit
{
    [Tooltip("�Ϲ� ���� �Ÿ�")]
    public float attackDistance = 25f;

    public LeafStorm leafStormInstance;
    public LeafRainSkill leafRainInstance;

    [SerializeField] private float skillGroggy = 3f;
    [SerializeField] private float LeafStormCoolTime = 20f;
    [SerializeField] private float LeafRainCoolTime = 20f;
    [SerializeField] private float BeamCoolTime = 20f;

    public ParticleSystem LeafStormParticle;
    public ParticleSystem LeafRainParticle;
    public ParticleSystem BeamParticle;
    public ParticleSystem InvincibleParticle;
    public Collider LaserCol;

    private BossState currentState;
    [SerializeField] private bool isSkillExecuting = false; // ��ų ���� ���� ���� �÷���

    private bool isInvincible = false; // ���� ���� ����
    private bool isInvincibleAnim = false; // ���� �ִϸ��̼� ���� ����
    // ��ȯ�� ���� ����Ʈ
    private List<GameObject> summonedMonsters = new List<GameObject>();
    // Ư�� ���� ������
    public GameObject specialMonsterPrefab;

    protected override void Awake()
    {
        base.Awake();
        agent.updateRotation = false;
        LaserCol.enabled = false; // Collider ��Ȱ��ȭ
    }
    private void Start()
    {
        StartCoroutine(Attack());
        StartCoroutine(UseSkills());
        currentState = BossState.IDLE;
    }
    private void Update()
    {
        target = GameObject.FindWithTag("Player").transform; // �÷��̾� �±׷� ����
        OnRangeAttack();

        switch (currentState)
        {
            case BossState.IDLE:
                HandleIdleState();
                break;
            case BossState.CHASE:
                Look(3f);
                HandleChaseState();
                break;
            case BossState.ATTACK:
                Look(1f); // �÷��̾ �ٶ�
                HandleAttackState();
                break;
            case BossState.LEAFSTORM:
            case BossState.LEAFRAIN:
            case BossState.BEAM:
                // ��ų�� �ڷ�ƾ���� ó��
                break;
            case BossState.INVINCIBLE:
                break;
            case BossState.DIE:
                // ���� ���� ó��
                break;
        }
        //test
        if (Input.GetKeyDown(KeyCode.Space))
            ChangeState(BossState.INVINCIBLE);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            ExitInvincibleState();
    }
    private void ChangeState(BossState newState)
    {
        Debug.Log($"[ChangeState] Called from: " +
            $"{new System.Diagnostics.StackTrace().GetFrame(1).GetMethod().Name}" +
            $" | Current: {currentState} -> New: {newState}");

        if (currentState == newState || isSkillExecuting || isInvincibleAnim)
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
                StartCoroutine(HandleSkillState(currentState));
                break;
            case BossState.LEAFRAIN:
                StartCoroutine(HandleSkillState(currentState));
                break;
            case BossState.BEAM:
                StartCoroutine(HandleSkillState(currentState));
                break;
            case BossState.INVINCIBLE:
                StartCoroutine(EnterInvincibleState());
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

    private IEnumerator HandleSkillState(BossState skill)
    {
        if (isSkillExecuting)
            yield break;

        isSkillExecuting = true;
        agent.isStopped = true;
        animator.SetBool("isChasing", false);
        animator.SetTrigger($"{skill}");
        switch (skill)
        {
            case BossState.LEAFSTORM:
                LeafStormParticle.Play();
                break;
            case BossState.LEAFRAIN:
                LeafRainParticle.Play();
                leafRainInstance.UseLeafRainSkill();
                break;
            case BossState.BEAM:
                yield return new WaitForSeconds(0.9f);
                BeamParticle.Play();
                yield return new WaitForSeconds(1f);
                LaserCol.enabled = true;
                break;
        }
        if (skill == BossState.BEAM)
        {
            yield return new WaitForSeconds(0.5f);
            LaserCol.enabled = false;
        }

        if (skill == BossState.LEAFRAIN)
        {
            yield return new WaitForSeconds(skillGroggy);
            animator.SetTrigger("Rainning");
            LeafRainParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        isSkillExecuting = false;
        yield return new WaitForSeconds(skillGroggy * 2);
        ChangeState(BossState.IDLE);
    }
    private IEnumerator UseSkills()
    {
        while (true)
        {
            // ���� ���� ��ų ����
            yield return new WaitForSeconds(LeafStormCoolTime);
            if (currentState != BossState.DIE)
                ChangeState(BossState.LEAFSTORM);
            // ���� ���� ��ų ����
            yield return new WaitForSeconds(LeafRainCoolTime);
            if (currentState != BossState.DIE)
                ChangeState(BossState.LEAFRAIN);
            yield return new WaitForSeconds(BeamCoolTime);
            if (currentState != BossState.DIE)
                ChangeState(BossState.BEAM);
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
        Dead();
    }
    public override void Dead()
    {
        Destroy(gameObject);
    }
    public override void Move()
    {
        if (currentState == BossState.LEAFSTORM
            || currentState == BossState.LEAFRAIN || currentState == BossState.BEAM)
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
                animator.SetBool("isAttack", true); // ���� �ִϸ��̼� Ȱ��ȭ

                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

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
    private IEnumerator EnterInvincibleState()
    {
        agent.isStopped = true;
        animator.SetBool("isChasing", false);

        isInvincible = true; // ���� ���·� ��ȯ
        isInvincibleAnim = true;

        animator.SetTrigger("INVINCIBLE");
        InvincibleParticle.Play(); // ���� ��ƼŬ ���

        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(3f);

        // ���� 3���� ��ȯ
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPosition = GetRandomPositionAroundBoss();
            GameObject monster = Instantiate(specialMonsterPrefab, randomPosition, Quaternion.identity);
            summonedMonsters.Add(monster);
        }

        // ���� ���� Ȯ�� ����
        StartCoroutine(CheckMonstersDefeated());

        // �ٽ� ��������
        isInvincibleAnim = false;
        agent.isStopped = false;
        ChangeState(BossState.IDLE);
    }
    private Vector3 GetRandomPositionAroundBoss()
    {
        float radius = 20f; // ��ȯ �ݰ�
        float minDistance = 4f; // ���� �� �ּ� �Ÿ�

        Vector3 randomPosition;
        int maxAttempts = 10; // �ִ� �õ� Ƚ��
        int attempts = 0;

        do
        {
            attempts++;
            float randomAngle = Random.Range(0, Mathf.PI * 2); // ���� ����

            // ���� ��ǥ ���
            float x = Mathf.Cos(randomAngle) * radius;
            float z = Mathf.Sin(randomAngle) * radius;

            randomPosition = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            // NavMesh ���� ��ȿ�� ��ġ ���ø�
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPosition = hit.position; // NavMesh ���� ��ġ ��ȯ

                // �ٸ� ���Ϳ��� ���� Ȯ��
                bool isFarEnough = true;
                foreach (var monster in summonedMonsters)
                {
                    if (Vector3.Distance(randomPosition, monster.transform.position) < minDistance)
                    {
                        isFarEnough = false;
                        break;
                    }
                }

                if (isFarEnough)
                    return randomPosition; // ������ �����ϸ� ��ġ ��ȯ
            }

        } while (attempts < maxAttempts);

        // ���� �� ���� ��ġ ��ȯ (���)
        return transform.position;
    }
    private IEnumerator CheckMonstersDefeated()
    {
        while (summonedMonsters.Count > 0)
            yield return new WaitForSeconds(1f); // �ֱ������� Ȯ��

        // ���� ���� ����
        ExitInvincibleState();
    }
    private void ExitInvincibleState()
    {
        isInvincible = false;
        InvincibleParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // ���� ��ƼŬ ����
    }
    public void RemoveMonster(GameObject monster)
    {
        if (summonedMonsters.Contains(monster))
            summonedMonsters.Remove(monster);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LaserCol.name == "LaserCollider")
        {
            if (other.CompareTag("Player"))
            {
                // ������ �÷��̾� ������ ó��
                Debug.Log($"Player hit by Laser!");
            }
        }
    }
}
