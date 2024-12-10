using EnemyController;
using HoonsCodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialFlower : EliteUnit
{
    private bool isCasting = false; // ���� ��ų ���� �� ����
    private bool IsPlayerInRange = false; // ���� ��ų ���� �� ����
    private bool shouldLook = true; // �÷��̾ �ٶ� �� �ִ� ��������

    public Collider attackRange; // ���� ����
    public ParticleSystem grass; // ��ƼŬ �ý���

    private FlowerState currentState;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform; // �÷��̾� �±׷� ����
        ChangeState(FlowerState.IDLE); // �ʱ� ���� ����
        StartCoroutine(LookTarget());
    }
    void Update()
    {
        // ���º� ����
        switch (currentState)
        {
            case FlowerState.IDLE:
                HandleIdleState();
                break;

            case FlowerState.CAST:
                HandleCastState();
                break;

            case FlowerState.DIE:
                break;
        }
        //test
        //if(Input.GetKeyDown(KeyCode.Escape))
        //    ChangeState(FlowerState.DIE);
    }
    private void ChangeState(FlowerState newState)
    {
        if (currentState == newState) return;

        Debug.Log($"State changed: {currentState} -> {newState}");
        currentState = newState;

        switch (newState)
        {
            case FlowerState.IDLE:
                StopAllCoroutines();
                animator.SetBool("isCasting", false); // ĳ���� �ִϸ��̼� ����
                break;

            case FlowerState.CAST:
                StartCoroutine(CastSkill()); // ��ų ���� ����
                break;

            case FlowerState.DIE:
                animator.SetTrigger("Die");
                StartCoroutine(Die());
                break;
        }
    }
    private void HandleIdleState()
    {
        if (IsPlayerInRange == true)
            ChangeState(FlowerState.CAST);
    }
    private void HandleCastState()
    {
        if (IsPlayerInRange == false)
            ChangeState(FlowerState.IDLE);
    }
    private IEnumerator CastSkill()
    {
        isCasting = true;

        while (currentState == FlowerState.CAST)
        {
            yield return new WaitForSeconds(attackDelay); // ���� ������
            if (IsPlayerInRange == false)
            {
                ChangeState(FlowerState.IDLE);
                yield break;
            }

            animator.SetBool("isCasting", true); // ĳ���� �ִϸ��̼� ����
            yield return new WaitForSeconds(1f);
            grass.Play(); // ��ƼŬ ����
            AttackPlayer();

            // 1�ʰ� �ٶ󺸱⸦ ����
            shouldLook = false;
            yield return new WaitForSeconds(1f);
            shouldLook = true;

            animator.SetBool("isCasting", false);
            yield return new WaitForSeconds(attackDelay); // ���� ������
        }

        isCasting = false;
    }
    private void AttackPlayer()
    {
        if (IsPlayerInRange == true)
        {
            Debug.Log("�ƾ�!");
        }
    }
    private IEnumerator LookTarget()
    {
        while (true)
        {
            if (animator.GetBool("isCasting") == true)
                yield return null;

            else if (shouldLook)
            {
                // �ٶ󺸴� ���� ����
                Look(3f);
            }
            yield return null; // �� ������ ���
        }
    }
    private IEnumerator Die()
    {
        StopCoroutine(LookTarget());
        gameObject.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2.0f); // ���� �ִϸ��̼� ���
        BossDryad boss = FindObjectOfType<BossDryad>();
        if (boss != null)
            boss.RemoveMonster(gameObject);
        Destroy(gameObject);
    }
    public override void Dead()
    {
        ChangeState(FlowerState.DIE);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = true; // �÷��̾ ������ ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerInRange = false; // �÷��̾ ������ ���
        }
    }
}
