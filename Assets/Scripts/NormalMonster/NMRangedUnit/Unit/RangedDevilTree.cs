using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedDevilTree : NMRangedUnit
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
        base.Update();
    }

    protected override void Attack()
    {
        Look();
        isAtk = true;
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        attackRange.gameObject.SetActive(true);
        attackRange.OnRange();//공격범위 표시
        yield return new WaitForSeconds(atkSpeed / 2);
        attackRange.gameObject.SetActive(false);
        GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletDamage = this.dmgValue;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletSpeed = this.bulletSpeed;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletLifeTime = this.bulletLifeTime;
        yield return new WaitForSeconds(atkSpeed / 2);
        isAtk = false;
    }

}
