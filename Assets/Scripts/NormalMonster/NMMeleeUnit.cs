using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMMeleeUnit : Character, ITakeDamage
{
    private enum State
    {
        Move,
        Attack,
        Die
    }
    private State state;
    [Tooltip("ȸ�� �ӵ�")]
    public float rotationSpeed;
    [Tooltip("���� ����")]
    [SerializeField] private float range;
    public Transform target;
    private NavMeshAgent agent;
    public EnemyHPbar hpBar;

    private bool isAtk = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent <CapsuleCollider>();
        animator = this.GetComponent<Animator>();
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
    }

    private void Start()
    {
        state = State.Move;
        isDead = false;
        agent.speed = moveSpeed;
        agent.angularSpeed = rotationSpeed;
        agent.acceleration = 1000f;
        //�÷��̾� ��ũ��Ʈ �����ͼ� Ÿ�ټ���
        GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        HpBarUpdate();
        float dirplayer = Vector3.Distance(transform.position, target.position);//Ÿ�ٰ��� �Ÿ�
        if (curHp <= 0 && isDead == false)//������ �ѹ� �ߵ�
        {
            isDead = true;
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

    private void HpBarUpdate()
    {
        hpBar.maxHp = this.maxHp;
        hpBar.currentHp = this.curHp;
        hpBar.GetHpBoost();
    }

    private void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    private void Look()//ȸ��
    {
        Vector3 direction = target.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void Attack()
    {
        Look();
        isAtk = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()//���� ������
    {
        //���ݹ��� ǥ��
        yield return new WaitForSeconds(atkSpeed / 2);
        Debug.Log("Player�� ����");
        //Ÿ�� ����
        //target.GetComponent<Player>().TakeDamage(dmgValue);
        yield return new WaitForSeconds(atkSpeed / 2);
        isAtk = false;
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
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
