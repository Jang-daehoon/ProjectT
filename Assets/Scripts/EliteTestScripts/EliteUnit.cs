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
    [SerializeField] protected float attackDelay = 2f; // ���ݰ� ������
    protected bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� ����

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
        if (target == null) return; // Ÿ���� ������ ����

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Y�� ȸ���� ����

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
