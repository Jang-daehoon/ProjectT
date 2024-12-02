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
        if (curHp <= 0)//죽을때 한번 발동
        {
            isDead = true;
        }
        if (isAtkMotion == false)
        {
            swordRender.enabled = false;
        }
        if (isAtkMotion == true)
        {
            swordRender.enabled = true;
        }
        if (isCollTime == true)
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
        isAtkMotion = true;
        agent.velocity = Vector3.zero;
        Look();
        atkRange.gameObject.SetActive(true);
        atkRange.OnRange();
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
        StartCoroutine(AtkCoolTime());
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        //공격범위 표시
        yield return new WaitForSeconds(atkSpeed * 0.5f);
        Debug.Log("Player를 공격");
        //타겟 공격
        //target.GetComponent<Player>().TakeDamage(dmgValue);
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
