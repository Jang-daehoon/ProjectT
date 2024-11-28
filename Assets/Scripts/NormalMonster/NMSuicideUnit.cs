using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMSuicideUnit : Character, ITakeDamage
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

    [Tooltip("공격 범위")]
    [SerializeField] private float range;
    [Tooltip("폭발 범위")]
    [SerializeField] private float attackRange;

    [Tooltip("폭발 파티클")]
    public ParticleSystem particle;

    [Tooltip("터지는데 걸리는 시간")]
    public float delay;

    public Transform target;
    private NavMeshAgent agent;

    public NMSuicideUnitRange boomRange;
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
        boomRange.radius = attackRange;
        boomRange.gameObject.SetActive(false);
    }

    //private void OnDrawGizmos() //공격범위표시 파티클 작업할때 쓰세요
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, range);
    //}

    private void Update()
    {
        HpBarUpdate();
        if (isAtk == true) return;//자폭 발동시 정지
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
        if (curHp <= 0 && isDead == false)//죽을때 한번 발동
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
        if (dirplayer > range && isDead == false)//공격범위내에 없으면 이동
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
        isAtk = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;//즉시 정지
        boomRange.gameObject.SetActive(true);
        boomRange.OnRange();//공격범위 표시
        Invoke("Boom", delay);//그자리에서 딜레이후 자폭
        //달려가서 자폭(?)
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
        Gizmos.DrawSphere(transform.position, range);
    }

    private void Boom()//자폭
    {
        animator.SetTrigger("Attack");
        Instantiate(particle, transform.position, transform.rotation);
        particle.Play();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (dirplayer <= attackRange && isDead == false)
        {
            Debug.Log("Player를 공격");
            //공격
        }
        Dead();
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
