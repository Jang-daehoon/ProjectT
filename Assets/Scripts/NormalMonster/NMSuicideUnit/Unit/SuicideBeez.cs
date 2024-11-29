using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBeez : NMSuicideUnit
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
        agent.isStopped = true;
        agent.velocity = Vector3.zero;//즉시 정지
        boomRange.gameObject.SetActive(true);
        boomRange.OnRange();//공격범위 표시
        Invoke("Boom", delay);//그자리에서 딜레이후 자폭
        //달려가서 자폭(?)
        //
    }

    private void Boom()//자폭
    {
        animator.SetTrigger("Attack");
        Instantiate(particle, transform.position, transform.rotation);
        particle.Play();
        var main = particle.main;
        main.stopAction = ParticleSystemStopAction.Destroy;
        float dirplayer = Vector3.Distance(transform.position, target.position);
        if (dirplayer <= attackRange && isDead == false)
        {
            Debug.Log("Player를 공격");
            //공격
            //
        }
        Dead();
    }
}
