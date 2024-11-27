using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMTree : Character, ITakeDamage
{
    private enum State
    {
        Move,
        Attack,
        Die
    }
    private State state;
    public Transform target;
    private NavMeshAgent agent;
    [Tooltip("공격 범위")]
    [SerializeField] private float range;

    private bool isAtk = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent <CapsuleCollider>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        state = State.Move;
        isDead = false;
        //플레이어 스크립트 가져와서 타겟설정
        GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
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

    private void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    private void Attack()
    {
        transform.LookAt(target);//뚝뚝끊기지만 일단은 공격직전 타겟방향으로 회전
        isAtk = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
        Debug.Log("Player를 공격");
        //타겟 공격
        //target.GetComponent<Player>().TakeDamage(dmgValue);
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        yield return new WaitForSeconds(atkSpeed);
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
