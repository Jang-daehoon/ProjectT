using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSkeleton : NMMeleeUnit
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (isAtk == true) return;
        base.Update();
    }

    protected override void Attack()
    {
        isAtk = true;
        Look();
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
}
