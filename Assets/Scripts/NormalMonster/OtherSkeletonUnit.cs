using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OtherSkeletonUnit : EnemyUint
{
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


}
