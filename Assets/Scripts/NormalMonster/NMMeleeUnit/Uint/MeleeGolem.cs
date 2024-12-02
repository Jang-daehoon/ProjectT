using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public class MeleeGolem : NMMeleeUnit
{
    private bool isGolemAttack = false;
    private bool isHit = false;
    private bool isGolemAttackCollTime = false;

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
        attackRange.gameObject.transform.position = this.transform.position;
        attackRange.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        HpBarUpdate();
        if (curHp <= 0)//죽을때 한번 발동
        {
            isDead = true;
        }
        if (isGolemAttack == true)
        {
            transform.position = Vector3.Lerp(transform.position, attackRange.end, Time.deltaTime * 2f);
        }
        if (isGolemAttackCollTime == true)
        {
            Move();
        }
        if (isDead == false)
        {
            if (isAtk == true) return;
        }
        base.Update();
    }

    protected override void Attack()
    { 
        isAtk = true;
        agent.velocity = Vector3.zero;
        Look();
        StartCoroutine(GolemMoveAttack());
        StartCoroutine(AtkCoolTime());
    }

    private IEnumerator GolemMoveAttack()
    {
        animator.SetBool("Idel", true);
        attackRange.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        attackRange.gameObject.SetActive(true);//이동범위 표시 On

        yield return new WaitForSeconds(1f);//돌진시작
        col.isTrigger = true;
        animator.SetBool("Idel", false);
        animator.SetBool("Attack", true);
        attackRange.gameObject.SetActive(false);//이동범위 표시 Off
        isGolemAttack = true;
        isHit = false;
        particle.gameObject.SetActive(true);
        particle.Play();

        yield return new WaitForSeconds(1f);
        col.isTrigger = false;
        isGolemAttack = false;
        attackRange.transform.position = this.transform.position;
        animator.SetBool("Attack", false);
        particle.gameObject.SetActive(false);
        particle.Stop();
        isGolemAttackCollTime = true;
    }

    private IEnumerator AtkCoolTime()
    {
        yield return new WaitForSeconds(attDelay);
        isAtk = false;
        isGolemAttackCollTime = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isGolemAttack == false) return;
        if (isHit == false)
        {
            if (other.CompareTag("Player"))
            {
                Vector3 dir = transform.position - other.transform.position;
                other.GetComponent<Rigidbody>().AddForce(-dir * 30f, ForceMode.Impulse);
                Debug.Log($"{other.name} Hit");
                //collision.gameObject.GetComponent<Player>().TakeDamage();
                //플레이어 공격
                isHit = true;
            }
        }
    }


}
