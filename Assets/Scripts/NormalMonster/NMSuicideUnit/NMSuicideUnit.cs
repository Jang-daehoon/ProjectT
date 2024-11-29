using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMSuicideUnit : EnemyUint
{
    [Tooltip("폭발 범위")]
    [SerializeField] protected float attackRange;

    [Tooltip("폭발 파티클")]
    public ParticleSystem particle;

    [Tooltip("터지는데 걸리는 시간")]
    public float delay;

    public NMSuicideUnitRange boomRange;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
    }

    protected virtual void Start()
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

    protected virtual void Update()
    {
        HpBarUpdate();
        if (isAtk == true) return;//자폭 발동시 정지
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
        if (curHp <= 0 && isDead == false)//죽을때 한번 발동
        {
            isDead = true;
            col.enabled = false;
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

    protected virtual void Attack()
    {
        
    }

}
