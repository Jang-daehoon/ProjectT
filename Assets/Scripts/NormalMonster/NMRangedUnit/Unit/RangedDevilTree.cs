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
        if (isDead == false) animator.SetBool("Idle", false);
        base.Update();
    }

    protected override void Attack()
    {
        isAtk = true;
        Look();
        StartCoroutine(AtkOff());
        StartCoroutine(CoolTime());
        if (isDead == false) animator.SetBool("Idle", true);
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        for (int i = 0; i < 3; i++)
        {
            attackRange[i].gameObject.SetActive(true);//사격범위 표시 Off
        }
        yield return new WaitForSeconds(atkSpeed);
        animator.SetBool("Idle", false);
        if (isDead == false) animator.SetTrigger("Attack");
        for (int i = 0; i < 3; i++)
        {
            attackRange[i].gameObject.SetActive(false);//사격범위 표시 Off
            attackRange[i].transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            Vector3 bulletPos = new Vector3(0, -30 + (i * 30), 0);
            GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
            nmbullet.transform.Rotate(bulletPos);
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletDamage = this.dmgValue;
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletSpeed = this.bulletSpeed;
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletLifeTime = this.bulletLifeTime;
        }
    }
    private IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(attDelay);
        isAtk = false;
    }

}
