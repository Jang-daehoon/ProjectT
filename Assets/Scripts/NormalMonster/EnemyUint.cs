using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoonsCodes;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyUint : Character, ITakeDamage
{
    protected enum State
    {
        Move,
        Attack,
        Die
    }
    protected float rotationSpeed = 1000f;
    [Tooltip("���� ����")]
    [SerializeField] protected float range;
    protected State state;
    public Transform target;
    protected NavMeshAgent agent;
    public EnemyHPbar hpBar;
    protected bool isAtk = false;


    protected void HpBarUpdate()
    {
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        hpBar.GetHpBoost();
    }

    protected void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    protected void Look()//ȸ��
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

    public void TakeDamage(float damage)//�������̽�
    {
        curHp -= damage;
        hpBar.HpBarUpdate();
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
