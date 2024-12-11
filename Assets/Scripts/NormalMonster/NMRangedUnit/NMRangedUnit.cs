using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMRangedUnit : EnemyUint
{
    [Tooltip("����ü")]
    public GameObject bullet;

    [Tooltip("����ü �ӵ�")]
    [SerializeField] protected float bulletSpeed;

    [Tooltip("����ü ���ӽð�")]
    [SerializeField] protected float bulletLifeTime;

    [Tooltip("�Ѿ� ���� ��ġ")]
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
        float dirplayer = Vector3.Distance(transform.position, target.position);//Ÿ�ٰ��� �Ÿ�
        if (curHp <= 0 && isDead == false)//������ �ѹ� �ߵ�
        {
            isDead = true;
            ChangeState(State.Die);
        }
        if (dirplayer <= range && isDead == false)//���ݹ������� ������ �������� ����
        {
            agent.isStopped = true;
            ChangeState(State.Attack);
        }
        if (dirplayer > range && isDead == false && isAtk == false)//���ݹ������� ������ �̵�
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
