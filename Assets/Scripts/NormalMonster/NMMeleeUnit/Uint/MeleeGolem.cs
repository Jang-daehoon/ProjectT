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
        //차징 + 이동공격범위표시
        //테일 렌더러 사용
        yield return new WaitForSeconds(1f);
        animator.SetBool("Idel", false);
        //달려가기 시작 && 공격애니메이션 시작
        animator.SetBool("Attack", true);
        isGolemAttack = true;
        isHit = false;
        //닿으면 공격
        //온콜라이더엔터로 공격
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
            //플레이어 공격
            isHit = true;
        }
    }

}
