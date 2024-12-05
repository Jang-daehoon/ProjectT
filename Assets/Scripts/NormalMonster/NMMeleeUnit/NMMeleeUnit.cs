using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMMeleeUnit : EnemyUint
{

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent <CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        attDelay = characterData.attackDelay;
        atkSpeed = characterData.attackSpeed;
        moveSpeed = characterData.moveSpeed;
        dmgValue = characterData.damage;
        maxHp = characterData.maxHp;
        curHp = maxHp;
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        target = GameManager.Instance.player.transform;
    }

    protected virtual void Start()
    {
        unitType = UnitType.Melee;
        state = State.Move;
        isDead = false;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = 1000f;
        //플레이어 스크립트 가져와서 타겟설정
        GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void Update()
    {
        float dirplayer = Vector3.Distance(transform.position, target.position);//타겟과의 거리
        if (isDead == true)//죽을때 한번 발동
        {
            col.enabled = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
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
