using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMRabby : Character
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

    [Tooltip("투사체")]
    public GameObject bullet;

    [Tooltip("투사체 속도")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("총알 생성 위치")]
    public Transform shootPos;

    private bool isAtk = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        state = State.Move;
        isDead = false;
        //플레이어 스크립트 가져와서 타겟설정

    }

    private void Update()
    {
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            agent.isStopped = true;
            ChangeState(State.Die);
        }
        if (dirplayer <= range && isDead == false)
        {
            agent.isStopped = true;
            ChangeState(State.Attack);
        }
        if (dirplayer > range && isDead == false)
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
        GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        nmbullet.GetComponent<NMBullet>().bulletDamage = this.dmgValue;
        nmbullet.GetComponent<NMBullet>().bulletSpeed = this.bulletSpeed;
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()
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

    public void TakeDamage(float damage)
    {
        curHp -= damage;
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
