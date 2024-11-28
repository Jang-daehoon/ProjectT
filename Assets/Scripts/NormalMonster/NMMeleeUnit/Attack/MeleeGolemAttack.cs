using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class MeleeGolemAttack : NMMeleeUnit
{
    private bool isOnGolemAttack = false;
    private SphereCollider coll;

    private void Start()
    {
        coll = gameObject.GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(isOnGolemAttack == true)
        {
            //Collider[] colls = Physics.OverlapSphere(coll,coll.radius); 
        }
    }

    public void GolemAttackON()
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
        //������ ����
        //���ݶ��̴����ͷ� ����
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
        isAtk = false;
    }

}
