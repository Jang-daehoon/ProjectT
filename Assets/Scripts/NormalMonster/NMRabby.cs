using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using HoonsCodes;

[RequireComponent(typeof(NavMeshAgent))]
public class NMRabby : Character
{
    private enum State
    {
        Move,
        Attack,
        Die
    }
    private State state;
    public Transform target;
    private NavMeshAgent agent;

    [Tooltip("���� ����")]
    [SerializeField] private float range;

    [Tooltip("����ü")]
    public GameObject bullet;

    [Tooltip("����ü �ӵ�")]
    [SerializeField] private float bulletSpeed;

    [Tooltip("�Ѿ� ���� ��ġ")]
    public Transform shootPos;

    private bool isAtk = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        col = this.GetComponent<CapsuleCollider>();
        animator = this.GetComponent<Animator>();
    }

    private void Start()
    {
        state = State.Move;
        isDead = false;
        //�÷��̾� ��ũ��Ʈ �����ͼ� Ÿ�ټ���

    }

    private void Update()
    {
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (curHp <= 0 && isDead == false)
        {
            isDead = true;
            agent.isStopped = true;
            ChangeState(State.Die);
        }
        if (dirplayer <= range && isDead == false)
        {
            agent.isStopped = true;
            ChangeState(State.Attack);
        }
        if (dirplayer > range && isDead == false)
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

    private void ChangeState(State changestate)
    {
        this.state = changestate;
    }

    private void Attack()
    {
        transform.LookAt(target);//�ҶҲ������� �ϴ��� �������� Ÿ�ٹ������� ȸ��
        isAtk = true;
        animator.SetTrigger("Attack");
        GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        nmbullet.GetComponent<NMBullet>().bulletDamage = this.dmgValue;
        nmbullet.GetComponent<NMBullet>().bulletSpeed = this.bulletSpeed;
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()
    {
        yield return new WaitForSeconds(atkSpeed);
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

    public void TakeDamage(float damage)
    {
        curHp -= damage;
        if (isAtk == false)
        {
            animator.SetTrigger("Damage");
        }
    }
}
