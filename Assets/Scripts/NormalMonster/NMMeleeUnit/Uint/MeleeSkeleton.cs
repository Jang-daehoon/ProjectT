using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : NMMeleeUnit
{
    public TrailRenderer swordRender;
    private bool isAtkMotion = false;
    private bool isCollTime = false;
    public NMSuicideUnitRange atkRange;

    protected override void Awake()
    {
        base.Awake();
        range = 2f;
    }

    protected override void Start()
    {
        base.Start();
        swordRender.enabled = false;
        atkRange.radius = range;
        atkRange.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        HpBarUpdate();
        if (curHp <= 0 && isDead == false)//������ �ѹ� �ߵ�
        {
            isDead = true;
            ChangeState(State.Die);
        }
        if (isAtkMotion == false)
        {
            swordRender.enabled = false;
        }
        if (isAtkMotion == true)
        {
            swordRender.enabled = true;
        }
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (isCollTime == true && dirplayer <= range && isDead == false)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else if (isCollTime == true && dirplayer > range && isDead == false)
        {
            Move();
        }
        if (isAtk == true) return;
        base.Update();
    }

    protected override void Attack()
    {
        isAtk = true;
        isAtkMotion = true;
        agent.velocity = Vector3.zero;
        Look();
        atkRange.gameObject.SetActive(true);
        atkRange.OnRange();
        if (isDead == false) animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
        StartCoroutine(AtkCoolTime());
    }

    private IEnumerator AtkOff()//���� ������
    {
        //���ݹ��� ǥ��
        yield return new WaitForSeconds(atkSpeed * 0.5f);
        Debug.Log("Player�� ����");
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (dirplayer <= range)
        {
            //Ÿ�� ����
            GameManager.Instance.player.TakeDamage(dmgValue);
        }
        else
        {
            Debug.Log("���� - ���ݹ��� ������ ����");
        }
        yield return new WaitForSeconds(atkSpeed * 0.5f);
        isAtkMotion = false;
        atkRange.gameObject.SetActive(false);
        isCollTime = true;
    }

    private IEnumerator AtkCoolTime()
    {
        yield return new WaitForSeconds(attDelay);
        isCollTime = false;
        isAtk = false;
    }
}
