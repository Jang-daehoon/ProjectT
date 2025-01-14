using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMRangedUnit : EnemyUint
{
    [Tooltip("투사체")]
    public GameObject bullet;

    [Tooltip("투사체 속도")]
    [SerializeField] protected float bulletSpeed;

    [Tooltip("투사체 지속시간")]
    [SerializeField] protected float bulletLifeTime;

    [Tooltip("총알 생성 위치")]
    public Transform shootPos;

    public UnitRange[] attackRange;

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
        range = 10f;
        curHp = maxHp;
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        target = GameManager.Instance.player.transform;
    }

    protected virtual void Start()
    {
        unitType = UnitType.Ranged;
        state = State.Move;
        isDead = false;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = 1000f;
        foreach (UnitRange a in attackRange)
        {
            a.tr.time = atkSpeed;
            a.gameObject.SetActive(false);
        }
    }

    protected virtual void Update()
    {
        HpBarUpdate();
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
        if (curHp <= 0 && isDead == false)//죽으면 한번 발동
        {
            isDead = true;
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
