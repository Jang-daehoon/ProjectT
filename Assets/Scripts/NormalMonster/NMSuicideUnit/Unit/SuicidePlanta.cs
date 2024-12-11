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
        agent.velocity = Vector3.zero;//��� ����
        boomRange.gameObject.SetActive(true);
        boomRange.OnRange();//���ݹ��� ǥ��
        Invoke("Boom", attDelay);//���ڸ����� �������� ����
    }
    private void Boom()//����
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
            //����
        }
        if (isDead == true) animator.SetTrigger("Die");
    }

}
