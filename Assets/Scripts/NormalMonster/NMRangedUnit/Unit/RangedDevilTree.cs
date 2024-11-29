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
        for(int i = 0; i < 3; i++)
        {
            attackRange[i].gameObject.transform.Rotate(new Vector3(0, -30 + (i * 30), 0));
        }
    }

    protected override void Attack()
    {
        isAtk = true;
        Look();
        animator.SetTrigger("Attack");
        StartCoroutine(AtkOff());
    }

    private IEnumerator AtkOff()//공격 딜레이
    {
        for (int i = 0; i < 3; i++)
        {
            attackRange[i].gameObject.SetActive(false);//사격범위 표시 Off
            attackRange[i].transform.position = this.transform.position;
        }
        yield return new WaitForSeconds(atkSpeed / 2);
        for (int i = 0; i < 3; i++)
        {
            attackRange[i].gameObject.SetActive(false);//사격범위 표시 Off
            Vector3 bulletPos = new Vector3(0, -30 + (i * 30), 0);
            GameObject nmbullet = Instantiate(bullet, shootPos.position, shootPos.rotation);
            nmbullet.transform.Rotate(bulletPos);
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletDamage = this.dmgValue;
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletSpeed = this.bulletSpeed;
            nmbullet.GetComponent<NMRangedUnitBullet>().bulletLifeTime = this.bulletLifeTime;
        }
        yield return new WaitForSeconds(atkSpeed / 2);
        isAtk = false;
    }

}
