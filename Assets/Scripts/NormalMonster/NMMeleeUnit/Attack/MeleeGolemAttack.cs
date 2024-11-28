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
        //차징 + 이동공격범위표시
        //테일 렌더러 사용
        yield return new WaitForSeconds(1f);
        animator.SetBool("Idel", false);
        //달려가기 시작 && 공격애니메이션 시작
        animator.SetBool("Attack", true);
        //닿으면 공격
        //온콜라이더엔터로 공격
        yield return new WaitForSeconds(1f);
        animator.SetBool("Attack", false);
        isAtk = false;
    }

}
