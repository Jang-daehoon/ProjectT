using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMSuicideUnit : EnemyUint
{
    [Tooltip("���� ����")]
    [SerializeField] protected float attackRange;

    [Tooltip("���� ��ƼŬ")]
    public ParticleSystem particle;

    [Tooltip("�����µ� �ɸ��� �ð�")]

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
        range = 1f;
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
        //�÷��̾� ��ũ��Ʈ �����ͼ� Ÿ�ټ���
        GameObject.FindGameObjectWithTag("Player");
        boomRange.radius = attackRange;
        boomRange.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        HpBarUpdate();
        if (isDead == true) return;
        if (curHp <= 0 && isDead == false)//������ �ѹ� �ߵ�
        {
            isDead = true;
            col.enabled = false;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            ChangeState(State.Die);
            animator.SetTrigger("Die");
        }
        if (isAtk == true)
        {
            transform.position = lookPos;
            return;//���� �ߵ��� ����
        }
        float dirplayer = Vector3.Distance(transform.position, target.position);//Ÿ�ٰ��� �Ÿ�
        if (dirplayer <= range && isDead == false)//���ݹ������� ������ �������� ����
        {
            agent.isStopped = true;
            ChangeState(State.Attack);
        }
        if (dirplayer > range && isDead == false)//���ݹ������� ������ �̵�
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
                animator.SetTrigger("Die");
                break;
        }
    }

    protected virtual void Attack()
    {
        
    }

}
