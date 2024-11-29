using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRabby : NMRangedUnit
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
        isAtk = true;
        Look();
        StartCoroutine(AtkOff());
       
    }

    private IEnumerator AtkOff()//���� ������
    {
        attackRange[0].gameObject.SetActive(true);//��ݹ��� ǥ�� On

        yield return new WaitForSeconds(atkSpeed / 2);
        attackRange[0].gameObject.SetActive(false);//��ݹ��� ǥ�� Off
        attackRange[0].transform.position = this.transform.position;
        animator.SetTrigger("Attack");

        GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletDamage = this.dmgValue;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletSpeed = this.bulletSpeed;
        nmbullet.GetComponent<NMRangedUnitBullet>().bulletLifeTime = this.bulletLifeTime;
        
        yield return new WaitForSeconds(atkSpeed / 2);
        isAtk = false;
    }
}
