using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMesh))]
public class EliteUnit : Character , ITakeDamage
{
    protected Transform target;
    protected NavMeshAgent agent;
    public EnemyHPbar hpBar;
    [SerializeField] protected float attackDelay = 2f; // 공격간 딜레이
    protected bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부

    protected void OnEnable()
    {
        BossInitStat();
        hpBar.maxHp = this.maxHp;
        HpBarUpdate();
    }
    protected void HpBarUpdate()
    {
        hpBar.currentHp = this.curHp;
    }
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Move()
    {
    }
    public override void Dead()
    {
    }
    protected void Look(float lookSpeed)
    {
        if (target == null) return; // 타겟이 없으면 리턴

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y축 회전을 고정

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
    }
    protected void BossInitStat()
    {
        maxHp = characterData.maxHp;
        curHp = maxHp;
        atkSpeed = characterData.attackSpeed;
        attackDelay = characterData.attackDelay;
        moveSpeed = characterData.moveSpeed;
        dmgValue = characterData.damage;
        isDead = false;
    }
    public void TakeDamage(float damage)
    {
        curHp -= damage;
        hpBar.HpBarUpdate();
    }
}
