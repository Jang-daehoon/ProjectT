using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MeleeGolem : NMRangedUnit
{
    private bool isGolemAttack = false;
    private bool isHit;

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
        base.Update();
    }

    protected override void Attack()
    {
        Look();
        isAtk = true;
        StartCoroutine(GolemMoveAttack());
    }

    private IEnumerator GolemMoveAttack()
    {
        animator.SetBool("Idel", true);
        //��¡ + �̵����ݹ���ǥ��
        //���� ������ ���
        yield return new WaitForSeconds(1f);
        animator.SetBool("Idel", false);
        //�޷����� ���� && ���ݾִϸ��̼� ����
        animator.SetBool("Attack", true);
        isGolemAttack = true;
        isHit = false;
        //������ ����
        //���ݶ��̴����ͷ� ����
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
        isGolemAttack = false;
        isAtk = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGolemAttack == false) return;
        if (isHit == false)
        {
            //collision.gameObject.GetComponent<Player>().TakeDamage();
            //�÷��̾� ����
            isHit = true;
        }
    }

}
