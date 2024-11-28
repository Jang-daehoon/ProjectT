using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMRangedUnit : Character, ITakeDamage
{
    private enum State
    {
        Move,
        Attack,
        Die
    }
    private State state;
    [Tooltip("회전 속도")]
    public float rotationSpeed;
    public Transform target;
    private NavMeshAgent agent;

    [Tooltip("공격 인식 범위")]
    [SerializeField] private float range;

    [Tooltip("투사체")]
    public GameObject bullet;

    [Tooltip("투사체 속도")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("투사체 지속시간")]
    [SerializeField] private float bulletLifeTime;

    [Tooltip("총알 생성 위치")]
    public Transform shootPos;

    public NMRangedUnitRange attackRange;
    public EnemyHPbar hpBar;

    private bool isAtk = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
    }

    private void Start()
    {
        state = State.Move;
        isDead = false;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = 1000f;
        //플레이어 스크립트 가져와서 타겟설정
        GameObject.FindGameObjectWithTag("Player");
        attackRange.gameObject.SetActive(false);

    }

    private void Update()
    {
        HpBarUpdate();
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
        if (curHp <= 0 && isDead == false)//죽으면 한번 발동
        {
            isDead = true;
            agent.isStopped = true;
            ChangeState(State.Die);
        }
        if (dirplayer <= range && isDead == false)//공격범위내에 들어오면 공격으로 변경
        {
            agent.isStopped = true;
            ChangeState(State.Attack);
        }
        if (dirplayer > range && isDead == false && isAtk == false)//공격범위내에 없으면 이동
        {
            ChangeState(State.Move);
        }

        switch (state)
        {
            case State.Attack:
                if (isAtk == false) Attack();
                break;
            case State.Move:
                Move();
                break;
            case State.Die:
                animator.SetTrigger("Die");
                break;
        }
    }

    private void HpBarUpdate()
    {
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        hpBar.GetHpBoost();
    }

    private void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    private void Look()//회전
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        Look();
        isAtk = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        attackRange.gameObject.SetActive(true);
        attackRange.OnRange();//공격범위 표시
        yield return new WaitForSeconds(atkSpeed / 2);
        attackRange.gameObject.SetActive(false);
        GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletDamage = this.dmgValue;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletSpeed = this.bulletSpeed;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletLifeTime = this.bulletLifeTime;
        yield return new WaitForSeconds(atkSpeed / 2);
        isAtk = false;
    }

    public override void Dead()
    {
        Destroy(this.gameObject);
    }

    public override void Move()
    {
        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
    }

    public void TakeDamage(float damage)//인터페이스
    {
        curHp -= damage;
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
