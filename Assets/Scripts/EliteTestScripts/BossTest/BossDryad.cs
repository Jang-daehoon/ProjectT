using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnemyController;
using HoonsCodes;
using System.IO;

public class BossDryad : EliteUnit
{
    [Tooltip("일반 공격 거리")]
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
    [SerializeField] private bool isSkillExecuting = false; // 스킬 상태 실행 여부 플래그

    private bool isInvincible = false; // 무적 상태 여부
    private bool isInvincibleAnim = false; // 무적 애니메이션 실행 여부
    // 소환된 몬스터 리스트
    private List<GameObject> summonedMonsters = new List<GameObject>();
    // 특수 몬스터 프리팹
    public GameObject specialMonsterPrefab;

    protected override void Awake()
    {
        base.Awake();
        agent.updateRotation = false;
        LaserCol.enabled = false; // Collider 비활성화
    }
    private void Start()
    {
        StartCoroutine(Attack());
        StartCoroutine(UseSkills());
        currentState = BossState.IDLE;
    }
    private void Update()
    {
        target = GameObject.FindWithTag("Player").transform; // 플레이어 태그로 참조
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
                Look(1f); // 플레이어를 바라봄
                HandleAttackState();
                break;
            case BossState.LEAFSTORM:
            case BossState.LEAFRAIN:
            case BossState.BEAM:
                // 스킬은 코루틴으로 처리
                break;
            case BossState.INVINCIBLE:
                break;
            case BossState.DIE:
                // 죽음 상태 처리
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
            ChangeState(BossState.IDLE); // 플레이어가 범위에서 벗어나면 IDLE 상태로 전환
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
            // 리프 스톰 스킬 시전
            yield return new WaitForSeconds(LeafStormCoolTime);
            if (currentState != BossState.DIE)
                ChangeState(BossState.LEAFSTORM);
            // 리프 레인 스킬 시전
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
            // 플레이어가 범위에 없으면 대기
            if (isPlayerInRange == false)
            {
                animator.SetBool("isAttack", false); // 공격 애니메이션 비활성화
                yield return null;
            }
            else
            {
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttack", true); // 공격 애니메이션 활성화

                yield return new WaitForEndOfFrame();
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                animator.SetBool("isAttack", false); // 공격 애니메이션 비활성화
                yield return new WaitForSeconds(attackDelay); // 공격 간 딜레이
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

        isInvincible = true; // 무적 상태로 전환
        isInvincibleAnim = true;

        animator.SetTrigger("INVINCIBLE");
        InvincibleParticle.Play(); // 무적 파티클 재생

        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(3f);

        // 몬스터 3마리 소환
        for (int i = 0; i < 3; i++)
        {
            Vector3 randomPosition = GetRandomPositionAroundBoss();
            GameObject monster = Instantiate(specialMonsterPrefab, randomPosition, Quaternion.identity);
            summonedMonsters.Add(monster);
        }

        // 몬스터 상태 확인 시작
        StartCoroutine(CheckMonstersDefeated());

        // 다시 패턴으로
        isInvincibleAnim = false;
        agent.isStopped = false;
        ChangeState(BossState.IDLE);
    }
    private Vector3 GetRandomPositionAroundBoss()
    {
        float radius = 20f; // 소환 반경
        float minDistance = 4f; // 몬스터 간 최소 거리

        Vector3 randomPosition;
        int maxAttempts = 10; // 최대 시도 횟수
        int attempts = 0;

        do
        {
            attempts++;
            float randomAngle = Random.Range(0, Mathf.PI * 2); // 랜덤 각도

            // 원형 좌표 계산
            float x = Mathf.Cos(randomAngle) * radius;
            float z = Mathf.Sin(randomAngle) * radius;

            randomPosition = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

            // NavMesh 위의 유효한 위치 샘플링
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                randomPosition = hit.position; // NavMesh 위의 위치 반환

                // 다른 몬스터와의 간격 확인
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
                    return randomPosition; // 조건을 만족하면 위치 반환
            }

        } while (attempts < maxAttempts);

        // 실패 시 보스 위치 반환 (백업)
        return transform.position;
    }
    private IEnumerator CheckMonstersDefeated()
    {
        while (summonedMonsters.Count > 0)
            yield return new WaitForSeconds(1f); // 주기적으로 확인

        // 무적 상태 해제
        ExitInvincibleState();
    }
    private void ExitInvincibleState()
    {
        isInvincible = false;
        InvincibleParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // 무적 파티클 중지
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
                // 레이저 플레이어 데미지 처리
                Debug.Log($"Player hit by Laser!");
            }
        }
    }
}
