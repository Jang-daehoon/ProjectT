using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : NMMeleeUnit
{
    public TrailRenderer swordRender;
    private bool isAtkMotion = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        swordRender.enabled = false;
    }

    protected override void Update()
    {
        if (isAtkMotion == false)
        {
            swordRender.enabled = false;
        }
        if (isAtkMotion == true)
        {
            swordRender.enabled = true;
            return;
        }

        base.Update();
    }

    protected override void Attack()
    {
        isAtk = true;
        isAtkMotion = true;
        Look();
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
        StartCoroutine(AtkCoolTime());
    }

    private IEnumerator AtkOff()//���� ������
    {
        //���ݹ��� ǥ��
        yield return new WaitForSeconds(0.3f);
        Debug.Log("Player�� ����");
        //Ÿ�� ����
        //target.GetComponent<Player>().TakeDamage(dmgValue);
        yield return new WaitForSeconds(0.7f);
        isAtkMotion = false;
    }

    private IEnumerator AtkCoolTime()
    {
        yield return new WaitForSeconds(atkSpeed);
        isAtk = false;
    }
}
