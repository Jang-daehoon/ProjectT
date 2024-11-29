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
    public float delay;

    public NMSuicideUnitRange boomRange;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
    }

    protected virtual void Start()
    {
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

    //private void OnDrawGizmos() //���ݹ���ǥ�� ��ƼŬ �۾��Ҷ� ������
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, range);
    //}

    protected virtual void Update()
    {
        HpBarUpdate();
        if (isAtk == true) return;//���� �ߵ��� ����
        float dirplayer = Vector3.Distance(transform.position, target.position);//Ÿ�ٰ��� �Ÿ�
        if (curHp <= 0 && isDead == false)//������ �ѹ� �ߵ�
        {
            isDead = true;
            col.enabled = false;
            agent.isStopped = true;
            ChangeState(State.Die);
        }
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
