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

    public NMSuicideUnitRange boomRange;

    private Vector3 lookPos;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        attDelay = characterData.attackDelay;
        atkSpeed = characterData.attackSpeed;
        moveSpeed = characterData.moveSpeed;
        dmgValue = characterData.damage;
        maxHp = characterData.maxHp;
        range = 2f;
        curHp = maxHp;
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        target = GameManager.Instance.player.transform;
    }

    protected virtual void Start()
    {
        unitType = UnitType.Suicide;
        state = State.Move;
        isDead = false;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = 1000f;
        boomRange.radius = attackRange;
        boomRange.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        HpBarUpdate();
        if (curHp <= 0 && isDead == false)//죽을때 한번 발동
        {
            isDead = true;
            ChangeState(State.Die);
        }
        if (isAtk == true && isDead == false)
        {
            transform.position = lookPos;
            return;//자폭 발동시 정지
        }
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
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
                if (isAtk == false)
                {
                    lookPos = transform.position;
                    Attack();
                }
                break;
            case State.Move:
                Move();
                break;
            case State.Die:
                col.enabled = false;
                agent.isStopped = true;
                agent.velocity = Vector3.zero;
                animator.SetTrigger("Die");
                break;
        }
    }

    protected virtual void Attack()
    {
        
    }

}
