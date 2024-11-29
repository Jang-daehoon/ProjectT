using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class MeleeGolem : NMMeleeUnit
{
    private bool isGolemAttack = false;
    private bool isHit;
    private bool isGolemAttackOn = false;

    public UnitRange attackRange;
    public ParticleSystem particle;

    protected override void Awake()
    {
        base.Awake();
        particle.gameObject.SetActive(false);
        particle.Stop();
    }

    protected override void Start()
    {
        base.Start();
        attackRange.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (isGolemAttack == true)
        {
            transform.position = Vector3.Lerp(transform.position, attackRange.end, Time.deltaTime);
        }
        if (isGolemAttackOn == true)
        {
            Move();
        }
        if (isAtk == true) return;
        base.Update();
    }

    protected override void Attack()
    { 
        isAtk = true;
        agent.velocity = Vector3.zero;
        Look();
        StartCoroutine(GolemMoveAttack());
        StartCoroutine(IsAtk());
    }

    private IEnumerator GolemMoveAttack()
    {
        animator.SetBool("Idel", true);
        attackRange.transform.position = this.transform.position;
        attackRange.gameObject.SetActive(true);//이동범위 표시 On

        yield return new WaitForSeconds(1f);//돌진시작
        animator.SetBool("Idel", false);
        animator.SetBool("Attack", true);
        attackRange.gameObject.SetActive(false);//이동범위 표시 Off
        isGolemAttack = true;
        isHit = false;
        particle.gameObject.SetActive(true);
        particle.Play();

        yield return new WaitForSeconds(1f);
        isGolemAttack = false;
        attackRange.transform.position = this.transform.position;
        animator.SetBool("Attack", false);
        particle.gameObject.SetActive(false);
        particle.Stop();
        isGolemAttackOn = true;
    }

    private IEnumerator IsAtk()
    {
        yield return new WaitForSeconds(atkSpeed);
        isAtk = false;
        isGolemAttackOn = false;
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
