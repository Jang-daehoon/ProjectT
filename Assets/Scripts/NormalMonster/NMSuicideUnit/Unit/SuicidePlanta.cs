using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoonsCodes;

public class SuicidePlanta : NMSuicideUnit
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
        col.isTrigger = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;//즉시 정지
        boomRange.gameObject.SetActive(true);
        boomRange.OnRange();//공격범위 표시
        Invoke("Boom", attDelay);//그자리에서 딜레이후 자폭
    }
    private void Boom()//자폭
    {
        if (isDead == false) animator.SetTrigger("Attack");
        Instantiate(particle, transform.position, transform.rotation);
        particle.Play();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (dirplayer <= attackRange && isDead == false)
        {
            GameManager.Instance.player.TakeDamage(dmgValue);
            //공격
        }
        if (isDead == true) animator.SetTrigger("Die");
    }

}
