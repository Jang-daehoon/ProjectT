using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoonsCodes;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUint : Character, ITakeDamage
{
    protected enum UnitType
    {
        Melee,
        Ranged,
        Suicide
    }
    protected enum State
    {
        Move,
        Attack,
        Die
    }
    protected float rotationSpeed = 500f;
    [Tooltip("공격 범위")]
    [SerializeField] protected float range;
    protected UnitType unitType;
    protected State state;
    public Transform target;
    protected NavMeshAgent agent;
    public EnemyHPbar hpBar;
    protected bool isAtk = false;
    protected float attDelay;

    protected void HpBarUpdate()
    {
        hpBar.currentHp = this.curHp;
        hpBar.GetHpBoost();
    }

    protected void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    protected void Look()//회전
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        hpBar.HpBarUpdate();
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
